using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTools
{
    public interface IPropertyDialogPage
    {
        void BeforeDeactivated(object dataObject);

        void BeforeActivated(object dataObject);
    }
}
