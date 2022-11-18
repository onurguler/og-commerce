using Og.Commerce.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Og.Commerce.Domain.Localization
{
    public class LanguageSlugAlreadyExistException : BusinessException
    {
        public LanguageSlugAlreadyExistException(string slug)
        {

        }
    }
}
