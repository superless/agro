using System;
using System.Collections.Generic;

namespace trifenix.agro.db.exceptions {
    public class Validation_Exception : Exception {

        public List<string> ErrorMessages;

    }
}