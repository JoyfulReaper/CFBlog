using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCBlog.Services
{
    public interface ISlugService
    {
        string UrlFriendly(string title);

        bool IsUnique(string slug);
    }
}
