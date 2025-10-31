using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core.UnitOfWork;
using LabBilling.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace LabBilling.Core.Services;

public interface IRandomDrugScreenService
{
    // Candidate Management
    Task<RandomDrugScreenPerson> AddCandidateAsync(RandomDrugScreenPerson person, IUnitOfWork uow = null);
    Task<RandomDrugScreenPerson> UpdateCandidateAsync(RandomDrugScreenPerson person, IUnitOfWork uow = null);
    Task<bool> DeleteCandidateAsync(int id, IUnitOfWork uow = null);
    Task<List<RandomDrugScreenPerson>> GetCandidatesByClientAsync(string clientMnem, bool includeDeleted = false, IUnitOfWork uow = null);
    Task<List<RandomDrugScreenPerson>> GetAllCandidatesAsync(bool includeDeleted = false, IUnitOfWork uow = null);
    Task<RandomDrugScreenPerson> GetCandidateByIdAsync(int id, IUnitOfWork uow = null);

    // Random Selection
    Task<List<RandomDrugScreenPerson>> SelectRandomCandidatesAsync(string clientMnem, int count, string shift = null, IUnitOfWork uow = null);

    // Import Operations
    Task<ImportResult> ImportCandidatesAsync(List<RandomDrugScreenPerson> candidates, string clientMnem, bool replaceAll = false, IUnitOfWork uow = null);

    // Client Management
    Task<List<string>> GetDistinctClientsAsync(IUnitOfWork uow = null);
    Task<List<string>> GetDistinctShiftsAsync(string clientMnem = null, IUnitOfWork uow = null);

    // Reporting
    Task<List<RandomDrugScreenPerson>> GetNonSelectedCandidatesAsync(string clientMnem, DateTime? fromDate = null, IUnitOfWork uow = null);
}

/// <summary>
/// Service for managing Random Drug Screen candidates and selections
/// </summary>
public sealed class RandomDrugScreenService : IRandomDrugScreenService
{
    private readonly IAppEnvironment _appEnvironment;

    public RandomDrugScreenService(IAppEnvironment appEnvironment)
    {
        _appEnvironment = appEnvironment ?? throw new ArgumentNullException(nameof(appEnvironment));
}

    /// <summary>
    /// Adds a new candidate to the system
    /// </summary>
    public async Task<RandomDrugScreenPerson> AddCandidateAsync(RandomDrugScreenPerson person, IUnitOfWork uow = null)
    {
        Log.Instance.Trace($"Entering - Adding candidate {person.Name} for client {person.ClientMnemonic}");
        uow ??= new UnitOfWorkMain(_appEnvironment);

   try
   {
       uow.StartTransaction();
    var result = await uow.RandomDrugScreenPersonRepository.AddAsync(person);
    uow.Commit();
      return result;
 }
        catch (Exception ex)
        {
            Log.Instance.Error(ex, "Error adding candidate");
        throw new ApplicationException("Error adding candidate", ex);
   }
    }

    /// <summary>
    /// Updates an existing candidate
    /// </summary>
 public async Task<RandomDrugScreenPerson> UpdateCandidateAsync(RandomDrugScreenPerson person, IUnitOfWork uow = null)
    {
        Log.Instance.Trace($"Entering - Updating candidate {person.Id}");
        uow ??= new UnitOfWorkMain(_appEnvironment);

        try
        {
 uow.StartTransaction();
      var result = uow.RandomDrugScreenPersonRepository.Update(person);
      uow.Commit();
            return await Task.FromResult(result);
        }
        catch (Exception ex)
        {
    Log.Instance.Error(ex, "Error updating candidate");
       throw new ApplicationException("Error updating candidate", ex);
        }
    }

    /// <summary>
  /// Soft deletes a candidate by setting the IsDeleted flag
    /// </summary>
    public async Task<bool> DeleteCandidateAsync(int id, IUnitOfWork uow = null)
    {
        Log.Instance.Trace($"Entering - Soft deleting candidate {id}");
        uow ??= new UnitOfWorkMain(_appEnvironment);

        try
        {
        uow.StartTransaction();
       var person = await uow.RandomDrugScreenPersonRepository.GetByKeyAsync(id);
            if (person == null)
 {
          Log.Instance.Warn($"Candidate {id} not found");
          return false;
         }

            person.IsDeleted = true;
            uow.RandomDrugScreenPersonRepository.Update(person);
            uow.Commit();
      return true;
      }
   catch (Exception ex)
        {
Log.Instance.Error(ex, "Error deleting candidate");
            throw new ApplicationException("Error deleting candidate", ex);
        }
    }

  /// <summary>
    /// Gets all candidates for a specific client
  /// </summary>
    public async Task<List<RandomDrugScreenPerson>> GetCandidatesByClientAsync(string clientMnem, bool includeDeleted = false, IUnitOfWork uow = null)
    {
   Log.Instance.Trace($"Entering - Getting candidates for client {clientMnem}");
        uow ??= new UnitOfWorkMain(_appEnvironment);

   try
        {
     return await uow.RandomDrugScreenPersonRepository.GetByClientAsync(clientMnem, includeDeleted);
        }
        catch (Exception ex)
        {
       Log.Instance.Error(ex, "Error getting candidates by client");
            throw new ApplicationException("Error getting candidates by client", ex);
        }
    }

    /// <summary>
    /// Gets all candidates in the system
    /// </summary>
    public async Task<List<RandomDrugScreenPerson>> GetAllCandidatesAsync(bool includeDeleted = false, IUnitOfWork uow = null)
    {
        Log.Instance.Trace($"Entering - Getting all candidates");
        uow ??= new UnitOfWorkMain(_appEnvironment);

        try
   {
            var all = await uow.RandomDrugScreenPersonRepository.GetAllAsync();
    if (includeDeleted)
      {
       return all.ToList();
   }
    return all.Where(p => !p.IsDeleted).ToList();
     }
        catch (Exception ex)
        {
  Log.Instance.Error(ex, "Error getting all candidates");
            throw new ApplicationException("Error getting all candidates", ex);
        }
    }

    /// <summary>
    /// Gets a specific candidate by ID
    /// </summary>
    public async Task<RandomDrugScreenPerson> GetCandidateByIdAsync(int id, IUnitOfWork uow = null)
    {
   Log.Instance.Trace($"Entering - Getting candidate {id}");
        uow ??= new UnitOfWorkMain(_appEnvironment);

        try
      {
      return await uow.RandomDrugScreenPersonRepository.GetByKeyAsync(id);
        }
     catch (Exception ex)
    {
         Log.Instance.Error(ex, "Error getting candidate by ID");
            throw new ApplicationException("Error getting candidate by ID", ex);
        }
    }

    /// <summary>
    /// Selects random candidates from the pool
  /// Uses cryptographically secure random number generation
    /// </summary>
    public async Task<List<RandomDrugScreenPerson>> SelectRandomCandidatesAsync(string clientMnem, int count, string shift = null, IUnitOfWork uow = null)
    {
        Log.Instance.Trace($"Entering - Selecting {count} random candidates for client {clientMnem}, shift {shift}");
  uow ??= new UnitOfWorkMain(_appEnvironment);

        if (string.IsNullOrEmpty(clientMnem))
            throw new ArgumentNullException(nameof(clientMnem), "Client mnemonic is required");

        if (count < 1)
            throw new ArgumentException("Count must be at least 1", nameof(count));

      try
    {
   // Get available candidates
            List<RandomDrugScreenPerson> pool;
        if (!string.IsNullOrEmpty(shift))
            {
       pool = await uow.RandomDrugScreenPersonRepository.GetByClientAndShiftAsync(clientMnem, shift, false);
     }
         else
         {
                pool = await uow.RandomDrugScreenPersonRepository.GetByClientAsync(clientMnem, false);
       }

          if (pool.Count == 0)
          {
  throw new ApplicationException($"No candidates available for client {clientMnem}" + 
     (string.IsNullOrEmpty(shift) ? "" : $" and shift {shift}"));
 }

       if (count > pool.Count)
            {
       throw new ArgumentException($"Requested count ({count}) exceeds available candidates ({pool.Count})", nameof(count));
            }

      // Perform cryptographically secure random selection using Fisher-Yates shuffle
        var selected = new List<RandomDrugScreenPerson>();
         var poolCopy = new List<RandomDrugScreenPerson>(pool);

  for (int i = 0; i < count; i++)
        {
     int index = GetSecureRandomIndex(poolCopy.Count);
      selected.Add(poolCopy[index]);
    poolCopy.RemoveAt(index);
         }

         // Update test dates for selected candidates
            uow.StartTransaction();
            var now = DateTime.Now;
            foreach (var candidate in selected)
    {
           candidate.TestDate = now;
           uow.RandomDrugScreenPersonRepository.Update(candidate);
            }
            uow.Commit();

       Log.Instance.Info($"Selected {selected.Count} candidates for client {clientMnem}");
            return selected;
}
        catch (Exception ex)
        {
  Log.Instance.Error(ex, "Error selecting random candidates");
            throw new ApplicationException("Error selecting random candidates", ex);
        }
    }

    /// <summary>
    /// Gets a cryptographically secure random index
    /// </summary>
    private int GetSecureRandomIndex(int maxValue)
    {
        if (maxValue <= 0)
            throw new ArgumentException("Max value must be greater than 0", nameof(maxValue));

        byte[] randomBytes = new byte[4];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
     }

      // Convert bytes to uint and use modulo to get index in range
   uint randomUInt = BitConverter.ToUInt32(randomBytes, 0);
   return (int)(randomUInt % (uint)maxValue);
    }

    /// <summary>
    /// Imports candidates from a list
    /// </summary>
    public async Task<ImportResult> ImportCandidatesAsync(List<RandomDrugScreenPerson> candidates, string clientMnem, bool replaceAll = false, IUnitOfWork uow = null)
    {
        Log.Instance.Trace($"Entering - Importing {candidates.Count} candidates for client {clientMnem}, replaceAll={replaceAll}");
        uow ??= new UnitOfWorkMain(_appEnvironment);

        var result = new ImportResult
  {
      TotalRecords = candidates.Count,
Errors = new List<string>(),
 Success = false
    };

        try
        {
         uow.StartTransaction();

     if (replaceAll)
         {
          // RTS Mode: Delete all existing records for client
            result.DeletedCount = await uow.RandomDrugScreenPersonRepository.SoftDeleteByClientAsync(clientMnem);
     
          // Insert all new records
    foreach (var candidate in candidates)
  {
     candidate.ClientMnemonic = clientMnem;
            await uow.RandomDrugScreenPersonRepository.AddAsync(candidate);
    result.AddedCount++;
           }
            }
       else
         {
   // BTM/Merge Mode: Mark missing as deleted, update existing, add new
            var existing = await uow.RandomDrugScreenPersonRepository.GetByClientAsync(clientMnem, false);
       var existingNames = existing.ToDictionary(e => e.Name.ToLower(), e => e);
     var importNames = candidates.Select(c => c.Name.ToLower()).ToList();

      // Mark candidates not in import as deleted
   foreach (var existingPerson in existing)
            {
         if (!importNames.Contains(existingPerson.Name.ToLower()))
       {
         existingPerson.IsDeleted = true;
        uow.RandomDrugScreenPersonRepository.Update(existingPerson);
     result.DeletedCount++;
          }
 }

   // Update or add candidates
    foreach (var candidate in candidates)
       {
                    candidate.ClientMnemonic = clientMnem;
       
  if (existingNames.TryGetValue(candidate.Name.ToLower(), out var existingPerson))
    {
   // Update existing
    existingPerson.Shift = candidate.Shift;
     existingPerson.IsDeleted = false; // Undelete if previously deleted
  uow.RandomDrugScreenPersonRepository.Update(existingPerson);
        result.UpdatedCount++;
    }
          else
            {
          // Add new
          await uow.RandomDrugScreenPersonRepository.AddAsync(candidate);
       result.AddedCount++;
       }
  }
          }

          uow.Commit();
    result.Success = true;
  Log.Instance.Info($"Import completed: {result.AddedCount} added, {result.UpdatedCount} updated, {result.DeletedCount} deleted");
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex, "Error importing candidates");
result.Errors.Add($"Import failed: {ex.Message}");
            result.Success = false;
}

        return result;
    }

    /// <summary>
    /// Gets distinct client mnemonics from candidates
    /// </summary>
    public async Task<List<string>> GetDistinctClientsAsync(IUnitOfWork uow = null)
    {
        Log.Instance.Trace($"Entering - Getting distinct clients");
   uow ??= new UnitOfWorkMain(_appEnvironment);

   try
        {
            return await uow.RandomDrugScreenPersonRepository.GetDistinctClientsAsync();
        }
        catch (Exception ex)
        {
          Log.Instance.Error(ex, "Error getting distinct clients");
            throw new ApplicationException("Error getting distinct clients", ex);
        }
    }

    /// <summary>
    /// Gets distinct shifts, optionally filtered by client
    /// </summary>
    public async Task<List<string>> GetDistinctShiftsAsync(string clientMnem = null, IUnitOfWork uow = null)
    {
        Log.Instance.Trace($"Entering - Getting distinct shifts for client {clientMnem}");
      uow ??= new UnitOfWorkMain(_appEnvironment);

     try
     {
       return await uow.RandomDrugScreenPersonRepository.GetDistinctShiftsAsync(clientMnem);
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex, "Error getting distinct shifts");
    throw new ApplicationException("Error getting distinct shifts", ex);
 }
    }

    /// <summary>
    /// Gets candidates who have not been selected since a given date
  /// </summary>
    public async Task<List<RandomDrugScreenPerson>> GetNonSelectedCandidatesAsync(string clientMnem, DateTime? fromDate = null, IUnitOfWork uow = null)
    {
Log.Instance.Trace($"Entering - Getting non-selected candidates for client {clientMnem} since {fromDate}");
     uow ??= new UnitOfWorkMain(_appEnvironment);

    try
    {
  var candidates = await uow.RandomDrugScreenPersonRepository.GetByClientAsync(clientMnem, false);
   
  if (fromDate.HasValue)
          {
         return candidates.Where(c => !c.TestDate.HasValue || c.TestDate.Value < fromDate.Value).ToList();
  }

   return candidates;
  }
        catch (Exception ex)
 {
Log.Instance.Error(ex, "Error getting non-selected candidates");
throw new ApplicationException("Error getting non-selected candidates", ex);
      }
    }
}

/// <summary>
/// Result of an import operation
/// </summary>
public class ImportResult
{
    public int TotalRecords { get; set; }
    public int AddedCount { get; set; }
    public int UpdatedCount { get; set; }
    public int DeletedCount { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
    public bool Success { get; set; }
}
