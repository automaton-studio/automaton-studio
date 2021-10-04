using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Automaton.Persistence
{
    /// <summary>
    /// Service to deal with funds
    /// </summary>
    public class AutomatonStore
    {
        #region Private Members

        private readonly AutomatonDbContext dbContext;

        #endregion

        #region Public Constructors

        public AutomatonStore(AutomatonDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        #endregion

        //#region Public Methods

        ///// <summary>
        ///// Retrieve paginated list of funds and their total.
        ///// </summary>
        ///// <param name="pageIndex">Page index to start from</param>
        ///// <param name="pageSize">Number of results per page</param>
        ///// <returns>Funds result with the total number and the actual results for the specified page</returns>
        //public async Task<FundsResult> GetFunds(int pageIndex, int pageSize, string searchText)
        //{
        //    if (pageIndex < 1)
        //    {
        //        throw new Exception("Page number should be greater than zero");
        //    }

        //    if (pageSize < 1)
        //    {
        //        throw new Exception("Page size should be greater than zero");
        //    }

        //    var total = dbContext.Funds.Count();

        //    // Different ways of returning results base on incoming search text
        //    var funds = string.IsNullOrEmpty(searchText) ?

        //         dbContext.Funds
        //            .OrderBy(x => x.Name)
        //            .Skip((pageIndex - 1) * pageSize)
        //            .Take(pageSize) :

        //        dbContext.Funds
        //            .Where(x => EF.Functions.Like(x.Name, $"%{searchText}%") || EF.Functions.Like(x.Description, $"%{searchText}%"))
        //            .OrderBy(x => x.Name)
        //            .Skip((pageIndex - 1) * pageSize)
        //            .Take(pageSize);

        //    var result = new FundsResult(total, funds);

        //    return await Task.FromResult(result);
        //}

        ///// <summary>
        ///// Retrieve fund by id
        ///// </summary>
        ///// <param name="fundId">Id to retrieve Fund</param>
        ///// <returns>Fund</returns>
        //public async Task<Fund> GetFund(int fundId)
        //{
        //    if (fundId < 1)
        //    {
        //        throw new Exception("Fund Id can not be negative");
        //    }

        //    var fund = await dbContext.Funds
        //        .Include(b => b.FundValues)
        //        .SingleOrDefaultAsync(x => x.Id == fundId);

        //    if (fund == null)
        //    {
        //        throw new Exception($"Fund with Id = {fundId} does not exists");
        //    }

        //    return fund;
        //}

        //#endregion
    }
}
