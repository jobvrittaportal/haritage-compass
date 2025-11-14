using System.Collections.Concurrent;

public static class CaptchaStore
{
  public static ConcurrentDictionary<string, string> Store
      = new ConcurrentDictionary<string, string>();
}
