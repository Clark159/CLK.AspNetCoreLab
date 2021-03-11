using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegisterModuleLab
{
    public class SettingContextModule : Autofac.Module
    {
        // Methods
        protected override void Load(ContainerBuilder builder)
        {
            #region Contracts

            if (builder == null) throw new ArgumentException(nameof(builder));

            #endregion

            // SettingContext
            {
                // Register
                builder.RegisterType<SettingContext>().As<SettingContext>();
            }
        }
    }
}
