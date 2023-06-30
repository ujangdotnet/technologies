using AutoMapper;
using xpos319.datamodels;
using xpos319.viewmodels;

namespace xpos319.Services
{
    public class CategoryTryService
    {
        private readonly XPOS_319Context db; //pembuatan line ini untuk apa ?
        VMResponse respon = new VMResponse(); //ini juga buat apa ?
        int IdUser = 1;

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

        //method manggil data
        public List<VMTblCategory> GetAllData() //fungsi ini buat apa ?
        {
            List<TblCategory> dataModel = db.TblCategories.Where(a => a.IsDelete == false).ToList();

            List<VMTblCategory> dataView = GetMapper().Map<List<VMTblCategory>>(dataModel);

            return dataView;
        }

        //method create
        public VMResponse Create(VMTblCategory dataView)
        {

            TblCategory dataModel = GetMapper().Map<TblCategory>(dataView);
            dataModel.CreateBy = IdUser;
            dataModel.CreateDate = DateTime.Now;
            dataModel.IsDelete = false;

            try
            { 
                db.Add(dataModel);
                db.SaveChanges();

                respon.Message = "Data Saved";
                respon.Entity = dataModel;
            }
            catch (Exception e)
            {
                respon.Success = false;
                respon.Message = "Failed Save Data: " + e.Message;
                respon.Entity = dataModel;
            }

            return respon;
        }

        public VMTblCategory GetById(int id)
        {
            TblCategory dataModel = db.TblCategories.Find(id);
            VMTblCategory dataView = GetMapper().Map<VMTblCategory>(dataModel);

            return dataView;
        }

        public VMResponse Edit(VMTblCategory dataView)
        {
            TblCategory dataModel = db.TblCategories.Find(dataView.Id);
            dataModel.NameCategory = dataView.NameCategory;
            dataModel.Description = dataView.Description;
            dataModel.UpdateBy = IdUser;
            dataModel.UpdateDate = DateTime.Now;

            try
            {
                db.Update(dataModel); //fuction update darimana ?
                db.SaveChanges(); //fuction save changes darimana ?

                respon.Message = "Data Success updated";
                respon.Entity = GetMapper().Map<VMTblCategory>(dataModel); //fuction getmapper() darimana ?

            }
            catch(Exception e)
            {
                respon.Success = false;
                respon.Message = "Failed edited: " + e.Message;
                respon.Entity = GetMapper().Map<VMTblCategory>(dataModel);
            }

            return respon;
        }

		public VMResponse Delete(VMTblCategory dataView)
		{
			TblCategory dataModel = db.TblCategories.Find(dataView.Id);
            dataModel.IsDelete = true;
            dataModel.UpdateBy = IdUser;
            dataModel.UpdateDate = DateTime.Now;

            try
            {
                db.Update(dataModel);
                db.SaveChanges();

				respon.Message = "Data Success removed";
				respon.Entity = GetMapper().Map<VMTblCategory>(dataModel); //fuction getmapper() darimana ?

			}
			catch (Exception e)
            {
				respon.Success = false;
				respon.Message = "Failed removed: " + e.Message;
				respon.Entity = GetMapper().Map<VMTblCategory>(dataModel);
			}

            return respon;

		}
	}
}
