using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SL_WCF
{
    [DataContract]
    [KnownType(typeof(ML.UsuarioML))]
    public class Result
    {
        [DataMember]
        public bool Correct { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
        [DataMember]
        public Exception Ex { get; set; }
        [DataMember]
        public object Object { get; set; }
        [DataMember]
        public List<Object> Objects { get; set; }

    }
}