using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SL_WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CRUDUsuario" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CRUDUsuario.svc or CRUDUsuario.svc.cs at the Solution Explorer and start debugging.
    public class CRUDUsuario : ICRUDUsuario
    {
        public SL_WCF.Result Add(ML.UsuarioML Usuario)
        {
            ML.Result resultAdd = BL.UsuarioBL.UsuarioAddTxtSPEF(Usuario);

            return new SL_WCF.Result
            {
                Correct = resultAdd.Correct,
                ErrorMessage = resultAdd.ErrorMessage,
                Ex = resultAdd.Ex,
                Object = resultAdd.Object,
                Objects = resultAdd.Objects,
            };
        }

        public SL_WCF.Result Update(ML.UsuarioML Usuario)
        {
            ML.Result resultUpdate = BL.UsuarioBL.UsuarioUpdateSPEF(Usuario);

            return new SL_WCF.Result
            {
                Correct = resultUpdate.Correct,
                ErrorMessage = resultUpdate.ErrorMessage,
                Ex = resultUpdate.Ex,
                Object = resultUpdate.Object,
                Objects = resultUpdate.Objects,
            };
        }

        public SL_WCF.Result Delete(int IdUsuario)
        {

               ML.Result resultDelete = BL.UsuarioBL.UsuarioDeleteSPEF(IdUsuario);

            return new SL_WCF.Result
            {
                Correct = resultDelete.Correct,
                ErrorMessage = resultDelete.ErrorMessage,
                Ex = resultDelete.Ex,
                Object = resultDelete.Object,
                Objects = resultDelete.Objects,
            };
        }

        public SL_WCF.Result GetById(int IdUsuario)
        {
            ML.Result resultGetById = BL.UsuarioBL.UsuarioGetByIdSPEF(IdUsuario);

            return new SL_WCF.Result
            {
                Correct = resultGetById.Correct,
                ErrorMessage = resultGetById.ErrorMessage,
                Ex = resultGetById.Ex,
                Object = resultGetById.Object,
                Objects = resultGetById.Objects,
            };
        }

        public SL_WCF.Result GetAll(ML.UsuarioML usuario)
        {
            ML.Result resultGetAll = BL.UsuarioBL.UsuarioGetAllSPEF(usuario);

            return new SL_WCF.Result
            {
                Correct = resultGetAll.Correct,
                ErrorMessage = resultGetAll.ErrorMessage,
                Ex = resultGetAll.Ex,
                Object = resultGetAll.Object,
                Objects = resultGetAll.Objects
            };
        }
    }
}
