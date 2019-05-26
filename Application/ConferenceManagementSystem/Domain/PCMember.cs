using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceManagementSystem.Entities
{
    class PCMember : User
    {
        private string Affiliation { get; set; }
        private string website { get; set; }
        private bool isReviewer { get; set; }
    }
}
