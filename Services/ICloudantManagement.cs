using System.Collections.Generic;
using System.Threading.Tasks;
using Cloudant.Models;

namespace Cloudant
{
    public interface IDatabaseProvider
    {
        Task<IResponseModel> CreateAsync(Contact model);
        Task<List<ContactViewModel>> ListAll();
        Task<DocumentJSON> GetDocumentById(string id);
        Task<IResponseModel> UpdateAsync(DocumentJSON model);
        Task<IResponseModel> DeleteAsync(string id, string rev);
    }
}