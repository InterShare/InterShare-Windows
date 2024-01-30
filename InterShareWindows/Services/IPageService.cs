using System;

namespace InterShareWindows.Services;

public interface IPageService
{
    Type GetPageType(string key);
}
