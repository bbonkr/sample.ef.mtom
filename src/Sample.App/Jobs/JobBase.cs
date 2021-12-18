﻿using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Sample.Data;

namespace Sample.App.Jobs
{
    public abstract class JobBase
    {
        public JobBase(AppDbContext context, ILogger logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected AppDbContext Context => context;

        protected ILogger Logger => logger;

        public abstract Task ExecuteAsync();

        private readonly AppDbContext context;
        private readonly ILogger logger;
    }
}
