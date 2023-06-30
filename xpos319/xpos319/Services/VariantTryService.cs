using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using xpos319.datamodels;
using xpos319.viewmodels;

namespace xpos319.Services
{
    public class VariantTryService
    {
        private readonly XPOS_319Context db;
        VMResponse respon = new VMResponse();
        int IdUser = 1;

        public VariantTryService(XPOS_319Context _db) //construktor ini buat apa ? (code yg akan dijalankan pertama kali)
        {
            this.db = _db;
        }

        public static IMapper GetMapper() //ini buat apa ? (auto mapper framework)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TblVariant, VMTblVariant>();
                cfg.CreateMap<VMTblVariant, TblVariant>();
            });

            IMapper mapper = config.CreateMapper();

            return mapper;
        }

        public List<VMTblVariant> GetAllData()
        {
            //List<TblVariant> dataModel = db.TblVariants.Where(a => a.IsDelete == false).ToList();

            List<VMTblVariant> dataView = (from v in db.TblVariants
                                           join c in db.TblCategories on v.IdCategory equals c.Id
                                           where v.IsDelete == false
                                           select new VMTblVariant
                                           {
                                               Id = v.Id,
                                               NameVariant = v.NameVariant,
                                               Description = v.Description,

                                               IdCategory = v.IdCategory,
                                               NameCategory = c.NameCategory
                                           }).ToList();

            return dataView;
        }

        public VMResponse Create(VMTblVariant dataView)
        {
            TblVariant dataModel = GetMapper().Map<TblVariant>( dataView );
            dataModel.CreateBy = IdUser;
            dataModel.CreateDate = DateTime.Now;
            dataModel.IsDelete = false;

            try
            {
                db.Add(dataModel);
                db.SaveChanges();
                 
                respon.Message = "Data Created";
                respon.Entity = dataModel;
            }
            catch (Exception)
            {
                respon.Success = false;
                respon.Message = "Failed to create";
                respon.Entity = dataView;
            }

            return respon;
        }

        //membuat method getbyid untuk menangkap data sesuai id (satu data, jadi gak perlu pake list)
        public VMTblVariant GetById(int id)
        {
            VMTblVariant dataView = (from v in db.TblVariants
                                           join c in db.TblCategories on v.IdCategory equals c.Id
                                           where v.IsDelete == false && v.Id == id
                                           select new VMTblVariant
                                           {
                                               Id = v.Id,
                                               NameVariant = v.NameVariant,
                                               Description = v.Description,

                                               IdCategory = v.IdCategory,
                                               NameCategory = c.NameCategory
                                           }).FirstOrDefault()!; //coba cari documentation nya

            return dataView;
        }

        //method respon edit (ketika di submit)
        public VMResponse Edit(VMTblVariant dataView)
        {
            TblVariant dataModel = db.TblVariants.Find(dataView.Id);
            dataModel.NameVariant = dataView.NameVariant;
            dataModel.IdCategory = dataView.IdCategory;
            dataModel.Description = dataView.Description;
            dataModel.UpdateBy = dataView.UpdateBy;
            dataModel.UpdateDate = dataView.UpdateDate;

            try
            {
                db.Update(dataModel);
                db.SaveChanges();

                respon.Message = "Data success save";
                respon.Entity = GetMapper().Map<VMTblVariant>(dataModel);
            }
            catch (Exception)
            {
                respon.Success = false;
                respon.Message = "Failed saved";
                respon.Entity = GetMapper().Map<VMTblVariant>(dataModel);
            }

            return respon;
        }

		//delete service
		public VMResponse Delete(VMTblVariant dataView)
		{
			TblVariant dataModel = db.TblVariants.Find(dataView.Id);
            dataModel.IsDelete = true;
			dataModel.UpdateBy = IdUser;
			dataModel.UpdateDate = DateTime.Now; 

			try
			{
				db.Update(dataModel);
				db.SaveChanges();

				respon.Message = "Data success removed";
				respon.Entity = GetMapper().Map<VMTblVariant>(dataModel);
			}
			catch (Exception)
			{
				respon.Success = false;
				respon.Message = "Failed removed";
				respon.Entity = GetMapper().Map<VMTblVariant>(dataModel);
			}

			return respon;
		}
	}
}
