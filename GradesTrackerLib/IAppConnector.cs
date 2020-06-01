using System;
using System.Collections.Generic;
using System.Text;

namespace GradesTrackerLib {
    interface IAppConnector {
        List<Assignment> GetData();
    }
}
