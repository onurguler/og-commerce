using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Og.Commerce.Core.Result;

public interface IResult
{
    ResultStatus Status { get; }
    IEnumerable<string> Errors { get; }
    List<ValidationError> ValidationErrors { get; }
    Type ValueType { get; }
    object GetValue();
}
