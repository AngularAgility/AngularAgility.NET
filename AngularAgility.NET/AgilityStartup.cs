using System;

namespace Angular.Agility
{
	public static class AgilityStartup
	{
		private static AgilityConfiguration config;

		internal static AgilityConfiguration Config
		{
			get
			{
				if (config == null)
					throw new InvalidOperationException("Must call AgilityStartup.Initialize() once at application startup.");
				return config;
			}
		}

		public static void Initialize(Action<AgilityConfiguration> configure = null)
		{
			const string dualInitError = "AgilityStartup.Initialize() can only be called once per AppDomain.";

			if (config != null)
				throw new InvalidOperationException(dualInitError);

			lock (typeof(AgilityStartup))
			{
				if (config != null)
					throw new InvalidOperationException(dualInitError);

				var c = new AgilityConfiguration();
				if (configure != null)
					configure(c);
				c.FinalizeConfiguration();
				config = c;
			}
		}
	}
}