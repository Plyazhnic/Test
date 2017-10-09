using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EXP.Entity;

namespace EXP.Core.Interface
{
    public interface IzStateRepository
    {
        /// <summary>
        /// get list state
        /// </summary>
        /// <returns></returns>
        List<zState> ListStates();
        /// <summary>
        /// Take State Name by ID
        /// </summary>
        /// <param name="StateId"></param>
        /// <returns></returns>
        string StateName(int StateId);
    }
}
