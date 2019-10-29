using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using trifenix.agro.db.interfaces.common;

namespace trifenix.agro.external.operations.tests.helper
{
    public class MoqHelper
    {

        
        
        public MoqHelper()
        {
            

            

        }


        

        public static MoqNotificationEvent NotificationEvent => new MoqNotificationEvent();

        public static MoqBarrack Barrack => new MoqBarrack();
        public static MoqPhenologicalEvent PhenologicalEvent => new MoqPhenologicalEvent();

        public static MoqCommonDb CommonDb => new MoqCommonDb();

        public static MoqUploadImage UpImage => new MoqUploadImage();



    }
}
