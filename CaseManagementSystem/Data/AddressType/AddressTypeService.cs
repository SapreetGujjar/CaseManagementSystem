using CaseManagementSystem.Data.AddressType;
using CMS.DL.DM;

namespace CaseManagementSystem.Data.AddressType
{
    public class AddressTypeService
    {
        private readonly string _connectionString = "";
        public AddressTypeService(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
        }
        #region GET
        public async Task<IEnumerable<AddressTypeViewModel>> GetAllAddressTypeAsync()
        {
            AddressTypeDM addressTypeDM = new AddressTypeDM(_connectionString);
            IEnumerable<CMS.DL.Model.AddressType> addressType = await addressTypeDM.GetAllAddressTypeAsync();

            return addressType.Select(c => new AddressTypeViewModel
            {
                Id = c.Id,
                Created = c.Created,
                CreatedBy = c.CreatedBy,
                Updated = c.Updated,
                UpdatedBy = c.UpdatedBy,
                Name = c.Name
            });
        }

        public async Task<AddressTypeViewModel> GetAddressTypeByIdAsync(Guid id)
        {
            AddressTypeDM addressTypeDM = new AddressTypeDM(_connectionString);
            CMS.DL.Model.AddressType addressType = await addressTypeDM.GetAddressTypeByIdAsync(id);

            if (addressType != null)
            {
                return new AddressTypeViewModel
                {
                    Id = addressType.Id,
                    Created = addressType.Created,
                    CreatedBy = addressType.CreatedBy,
                    Updated = addressType.Updated,
                    UpdatedBy = addressType.UpdatedBy,
                    Name = addressType.Name
                };
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region INSERT/UPDATE/DELETE

        public async Task<int> InsertAddressTypeAsync(AddressTypeViewModel addressTypeViewModel)
        {
            AddressTypeDM addressTypeDM = new AddressTypeDM(_connectionString);
            int result = await addressTypeDM.InsertAddressTypeAsync(new CMS.DL.Model.AddressType
            {
                Created = addressTypeViewModel.Created,
                CreatedBy = addressTypeViewModel.CreatedBy,
                Updated = addressTypeViewModel.Updated,
                UpdatedBy = addressTypeViewModel.UpdatedBy,
                Name = addressTypeViewModel.Name
            });

            return result;
        }

        public async Task<int> UpdateAddressTypeAsync(AddressTypeViewModel addressTypeViewModel)
        {
            AddressTypeDM addressTypeDM = new AddressTypeDM(_connectionString);
            int result = await addressTypeDM.UpdateAddressTypeAsync(new CMS.DL.Model.AddressType
            {
                Id = addressTypeViewModel.Id,
                Created = addressTypeViewModel.Created,
                CreatedBy = addressTypeViewModel.CreatedBy,
                Updated = addressTypeViewModel.Updated,
                UpdatedBy = addressTypeViewModel.UpdatedBy,
                Name = addressTypeViewModel.Name
            });

            return result;
        }

        public async Task<int> DeleteAddressTypeAsync(Guid id)
        {
            AddressTypeDM addresstypeDM = new AddressTypeDM(_connectionString);
            int result = await addresstypeDM.DeleteAddressTypeAsync(id);

            return result;
        }

        #endregion

    }
}
