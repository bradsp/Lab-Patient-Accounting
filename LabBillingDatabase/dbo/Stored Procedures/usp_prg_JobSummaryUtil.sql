/*
Job Summary
By Adnan Korkmaz, 2011/12/07 


Some background: I am not a DBA and it's my first contribution here. 
Sorry for any typos / errors if any in advance. I develop an application 
which runs on several SQL servers where I have many jobs with different 
schedules on each server. To be able to monitor all these servers at once, 
I wrote an Asp page which executes this script and summarizes the results.

Installation: You can create the script in whatever database you like, 
while I preferred master.


Parameters: (All parameters are optional)

@JobName: The exact name of the Job you want to filter. If omitted, all mathing jobs listed. 
@ShowDisabled: Inludes or excludes disabled jobs. 
@ShowUnscheduled: Includes or excludes jobs who have no schedule at all. 
@JobThresholdSec: If this parameters is positive, shows only the jobs with Last duration above it. 
@AvgExecThresholdSec: If this parameters is positive, shows only the jobs with Average duration above it. 

Output Columns:

JobName : Name of the job 
LastStep : The last step executed / currently executing 
Info : General information about the status of the job (Running, Disabled, No steps, 
Not scheduled, Never Run, Failed, Succeeded, Retry, Canceled or Unknown) 
DatabaseName : The db name of the job 
Enabled, ScheduleCount, StepCount : Obvious I guess. 
StartDate : Execution start 
FinishDate : Execution end (if ended) 
DurationSec : Seconds between StartDate and FinishDate. If currently running, 
between StartDate and GetDate(). (i.e. 3828) 
DurationSecFormatted : DurationSec above, human-readably formatted (i.e. 01:03:48) 
avgDurationSec : Average execution time in seconds. 
avgDurationSecFormatted : avgDurationSec above, formatted in hh:mm:ss (i.e. 02:19:12) 
DurationRatio : The current duration divided by average duration, 
as a percentage. This might give hint that something might be wrong with the job, 
if above a threshold of your own. For example a DurationRatio of 140 means that 
the job is taking / has taken a time 40 percent longer than it normally used to. 
NextRunDate : The next time the job will run.  
StepCommand : The text for the step (SQL / command line, etc) Becomes handy 
if you have really many jobs, whose details can't be understood from its name. 
HistoryMessage: The last history message. 
Samples:

Default usage lists all enabled jobs with a schedule:

exec usp_JobSummaryUtil 

You can filter to see only one job at a time:

exec JobSummaryUtil 'syspolicy_purge_history' 

List all jobs:

exec usp_JobSummaryUtil null, 1, 1 

List all jobs, whose last execution time is above 1 minute:

exec JobSummaryUtil null, 1, 1, 60 

List all jobs, whose average execution time is above 1 hour:

exec JobSummaryUtil null, 1, 1, 0, 3600 
*/

CREATE PROCEDURE [dbo].[usp_prg_JobSummaryUtil]
@JobName VARCHAR(255) = null, -- Optional job name filter
@ShowDisabled bit = 0, -- Include disabled jobs?
@ShowUnscheduled bit = 0, -- Include Unscheduled jobs?
@JobThresholdSec INT = 0, -- If positive, show only the jobs with LAST duration above this.
@AvgExecThresholdSec INT = 0 -- If positive, show only the jobs with AVERAGE duration above this.
AS
SELECT *
FROM 
( 
 SELECT JobName, ISNULL(LastStep,'') LastStep,
 CASE WHEN StartDate IS NOT NULL AND FinishDate IS NULL THEN 'Running' 
 WHEN Enabled = 0 THEN 'Disabled' 
 WHEN StepCount = 0 THEN 'No steps' 
 WHEN RunStatus IS NOT NULL THEN RunStatus 
 WHEN ScheduleCount = 0 THEN 'Not scheduled' 
 ELSE 'UNKNOWN' END Info,
 DatabaseName, Enabled, ScheduleCount, StepCount, 
 StartDate, FinishDate, DurationSec, 
 RIGHT('0'+convert(varchar(5),DurationSec/3600),2)+':'+RIGHT('0'+convert(varchar(5),DurationSec%3600/60),2)+':'+ RIGHT('0'+convert(varchar(5),(DurationSec%60)),2) DurationSecFormatted, 
 avgDurationSec,
 RIGHT('0'+convert(varchar(5),avgDurationSec/3600),2)+':'+RIGHT('0'+convert(varchar(5),avgDurationSec%3600/60),2)+':'+ RIGHT('0'+convert(varchar(5),(avgDurationSec%60)),2) avgDurationSecFormatted, 
 CASE WHEN (DurationSec IS NULL OR ISNULL(avgDurationSec, 0) = 0) THEN 0 ELSE CONVERT(DECIMAL(18,2), (100*CAST(DurationSec AS DECIMAL)) / CAST (avgDurationSec as DECIMAL)) END AS DurationRatio, 
 NextRunDate, 
 StepCommand, 
 HistoryMessage 
 FROM 
 ( 
 SELECT j.name JobName,j.enabled Enabled, 
 (select COUNT(1) from msdb..sysjobschedules jss where jss.job_id = j.job_id) ScheduleCount, 
 (select COUNT(1) from msdb..sysjobsteps jps where jps.job_id = j.job_id) StepCount, 
 ls1.job_history_id HistoryID, 
 ls1.start_execution_date StartDate, 
 ls1.stop_execution_date FinishDate, 
 ls1.last_executed_step_id LastStepID, 
 DATEDIFF(SECOND, ls1.start_execution_date, CASE WHEN ls1.stop_execution_date IS NULL THEN GETDATE() ELSE ls1.stop_execution_date END) DurationSec, 
 ISNULL(avgSec, 0) avgDurationSec, 
 ls1.next_scheduled_run_date NextRunDate, 
 st.step_name LastStep, st.command StepCommand, st.database_name DatabaseName, 
 h.message HistoryMessage, 
 CASE WHEN h.job_id IS NULL THEN 'Never Run' ELSE 
 CASE h.run_status 
 WHEN 0 THEN 'Failed' 
 WHEN 1 THEN 'Succeeded' 
 WHEN 2 THEN 'Retry' 
 WHEN 3 THEN 'Canceled' END END RunStatus, 
 h.run_date rawRunDate, 
 h.run_time rawRunTime, 
 h.run_duration rawRunDuration 
 FROM msdb..sysjobactivity ls1 (NOLOCK) 
 INNER JOIN msdb..sysjobs j (NOLOCK) ON ls1.job_id = j.job_id 
 INNER JOIN 
 ( 
 SELECT job_id JobID, MAX(session_id) LastSessionID 
 FROM msdb..sysjobactivity (NOLOCK) 
 GROUP BY job_id 
 ) ls2 ON ls1.job_id = ls2.JobID and ls1.session_id = ls2.LastSessionID 
 LEFT OUTER JOIN msdb..sysjobsteps st (NOLOCK) ON st.job_id = j.job_id and ls1.last_executed_step_id = st.step_id 
 LEFT OUTER JOIN msdb..sysjobhistory h (NOLOCK) ON h.instance_id = ls1.job_history_id 
 LEFT OUTER JOIN 
 ( 
 SELECT j.job_id JobID, SUM(h.avgSecs) avgSec 
 FROM msdb..sysjobs j (NOLOCK) 
 INNER JOIN 
 ( 
 SELECT job_id, step_id, AVG(run_duration/10000*3600 + run_duration%10000/100*60 + run_duration%100) avgSecs 
 FROM msdb..sysjobhistory 
 WHERE step_id > 0 AND run_status = 1 
 GROUP BY job_id,step_id 
 ) h on j.job_id = h.job_id 
 GROUP BY j.job_id 
 ) jobavg ON jobavg.JobID = j.job_id 
 )jj 
 WHERE (@ShowDisabled = 1 OR (StartDate IS NOT NULL AND FinishDate IS NULL) OR Enabled = 1) 
 AND (@JobName IS NULL OR JobName = @JobName) 
 AND (@ShowUnscheduled = 1 OR (StartDate IS NOT NULL AND FinishDate IS NULL) OR ScheduleCount > 0) 
 AND (@JobThresholdSec = 0 OR DurationSec >= @JobThresholdSec) 
 AND (@AvgExecThresholdSec = 0 OR avgDurationSec >= @AvgExecThresholdSec) 
)x 
ORDER BY CASE Info 
WHEN 'Running' THEN 0 
WHEN 'Failed' THEN 1 
WHEN 'Retry' THEN 2
WHEN 'Succeeded' THEN 3
WHEN 'Canceled' THEN 4
WHEN 'No steps' THEN 5 
WHEN 'Not scheduled' THEN 6
WHEN 'Disabled' THEN 7
WHEN 'Never Run' THEN 8
WHEN 'UNKNOWN' THEN -1
ELSE -2 END, NextRunDate--, JobName
