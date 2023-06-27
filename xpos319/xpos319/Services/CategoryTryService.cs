using AutoMapper;
using xpos319.datamodels;
using xpos319.viewmodels;

namespace xpos319.Services
{
	public class CategoryTryService
	{
		private readonly XPOS_319Context db; //pembuatan line ini untuk apa ?
		VMResponse respon = new VMResponse(); //ini juga buat apa ?

        public CategoryTryService(XPOS_319Context _db)
        {
            this.db = _db;
        }

        public static IMapper GetMapper() //ini buat apa ?
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TblCategory, VMTblCategory>();
                cfg.CreateMap<VMTblCategory, TblCategory>();
            });

            IMapper mapper = config.CreateMapper();

            return mapper;
        }
        public List<VMTblCategory> GetAllData() //fungsi ini buat apa ?
        {
            List<TblCategory> dataModel = db.TblCategories.Where(a => a.IsDelete == false).ToList();

            List<VMTblCategory> dataView = GetMapper().Map<List<VMTblCategory>>(dataModel);

            return dataView;
        }
    }
}
