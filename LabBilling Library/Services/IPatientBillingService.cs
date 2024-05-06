using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LabBilling.Core.Services;
public interface IPatientBillingService
{
    event EventHandler<ProgressEventArgs> ProgressIncrementedEvent;

    PatientStatement AddPatientStatement(PatientStatement record);
    bool BatchPreviouslyRun(string batchNo);
    void CompileStatements(DateTime throughDate);
    Task CompileStatementsAsync(DateTime throughDate);
    void CompileStatementsNew(DateTime throughDate);
    Task CompileStatementsNewAsync(DateTime throughDate);
    string CreateStatementFile(DateTime throughDate);
    Task<string> CreateStatementFileAsync(DateTime throughDate);
    Task<string> GenerateCollectionsFileAsync(IEnumerable<BadDebt> records);
    BadDebt GetCollectionRecord(string accountNo);
    PatientStatement GetStatement(long statementNumber);
    List<PatientStatement> GetStatements(string accountNo);
    List<PatientStatement> GetStatementsByBatch(string batch);
    int RegenerateCollectionsFile(DateTime tDate);
    BadDebt SaveCollectionRecord(BadDebt badDebt);
    string SendToCollections();
    Task<string> SendToCollectionsAsync();
}