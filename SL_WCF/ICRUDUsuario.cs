using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SL_WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICRUDUsuario" in both code and config file together.
    [ServiceContract]
    public interface ICRUDUsuario
    {
        [OperationContract]
        SL_WCF.Result Add(ML.UsuarioML Usuario);
        [OperationContract]
        SL_WCF.Result Update(ML.UsuarioML Usuario);
        [OperationContract]
        SL_WCF.Result Delete(int IdUsuario);
        [OperationContract]
        SL_WCF.Result GetById(int IdUsuario);
        [OperationContract]
        SL_WCF.Result GetAll(ML.UsuarioML usuario);


    }
}
