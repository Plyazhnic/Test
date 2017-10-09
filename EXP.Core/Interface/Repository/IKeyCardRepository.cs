using EXP.Entity;
using System.Collections.Generic;

namespace EXP.Core.Interface
{
    public interface IKeyCardRepository
    {
        /// <summary>
        /// List Key Cards For Page
        /// </summary>
        /// <returns></returns>
        List<KeyCard> ListKeyCardsForPage(TableList listParam);
        /// <summary>
        /// Get Key Cards Count
        /// </summary>
        /// <returns></returns>
        int GetKeyCardsCount(TableList listParam);
        /// <summary>
        /// Get Key Card By Id
        /// </summary>
        /// <param name="Cardid"></param>
        /// <returns></returns>
        KeyCard GetKeyCardById(int Cardid);
        /// <summary>
        /// Create Key Card
        /// </summary>
        /// <param name="KeyCard"></param>
        /// <returns></returns>
        bool CreateKeyCard(KeyCard KeyCard);
        /// <summary>
        /// Update Key Card
        /// </summary>
        /// <param name="KeyCard"></param>
        /// <returns></returns>
        bool UpdateKeyCard(KeyCard KeyCard);
        /// <summary>
        /// Delete Key Card
        /// </summary>
        /// <param name="KeyCardId"></param>
        void DeleteKeyCard(int KeyCardId);

    }
}
