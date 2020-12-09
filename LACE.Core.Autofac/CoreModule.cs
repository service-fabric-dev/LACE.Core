using System;
using Autofac;
using Autofac.Builder;
using LACE.Core.Abstractions.Content;
using LACE.Core.Content;

namespace LACE.Core.Autofac
{
    public class CoreModule : Module
    {
        private readonly CoreOverrides _overrides;

        public CoreModule()
        {
        }

        public CoreModule(CoreOverrides overrides)
        {
            _overrides = overrides;
        }

        protected override void Load(ContainerBuilder builder)
        {
            try
            {
                if (_overrides?.EmbeddedContentLoader != null)
                {
                    _overrides.EmbeddedContentLoader(builder);
                }
                else
                {
                    builder.RegisterType<EmbeddedContentLoader>()
                        .As<IEmbeddedContentLoader>();
                }


            }
            finally
            {
                base.Load(builder);
            }
        }
    }

    // TODO: constrain how the overrides behave, can  currently register anything
    public class CoreOverrides
    {
        /// <summary>
        /// Gets or sets an override action that registers an <see cref="IEmbeddedContentLoader"/>
        /// </summary>
        public Action<ContainerBuilder> EmbeddedContentLoader { get; set; }
    }
}
