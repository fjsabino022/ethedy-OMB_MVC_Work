using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura
{
  public class OMBBusinessRuleException : Exception
  {
    public OMBBusinessRuleException(string msg) : base(msg) { }
  }

  public class OMBSecurityException : Exception
  {
    public OMBSecurityException(string msg) : base(msg) { }
  }

  public class OMBDataPersistenceError : Exception
  {
    public OMBDataPersistenceError(string msg, Exception inner) : base(msg, inner) { }
  }
}
