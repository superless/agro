using System;
using System.Collections.Generic;
using System.Text;
using trifenix.connect.mdm.enums;
using trifenix.connect.mdm.ts_model;

namespace trifenix.connect.mdm.resources
{
    public interface IMdmDocumentation {

        EntitySearchDisplayInfo GetInfoFromProperty(KindProperty property, int index);

        EntitySearchDisplayInfo GetInfoFromEntity(int index);

    }
}
