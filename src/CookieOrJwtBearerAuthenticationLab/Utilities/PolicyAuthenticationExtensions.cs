using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookieOrJwtBearerAuthenticationLab
{
    public static class PolicyAuthenticationExtensions
    {
        // Methods
        public static AuthenticationBuilder AddPolicy(this AuthenticationBuilder builder, Action<PolicyAuthenticationOptions> configureOptions)
        {
            #region Contracts

            if (builder == null) throw new ArgumentException(nameof(builder));
            if (configureOptions == null) throw new ArgumentException(nameof(configureOptions));

            #endregion

            // Options
            var options = new PolicyAuthenticationOptions();
            {
                // Configure
                configureOptions(options);
            }

            // Require
            if (string.IsNullOrEmpty(options.DefaultScheme) == true) throw new InvalidOperationException($"{nameof(options.DefaultScheme)}=null");
            if (options.DefaultScheme == PolicyAuthenticationDefaults.AuthenticationScheme) throw new InvalidOperationException($"{nameof(options.DefaultScheme)}={PolicyAuthenticationDefaults.AuthenticationScheme}");

            // MainPolicy
            builder.AddPolicyScheme(PolicyAuthenticationDefaults.AuthenticationScheme, null, policySchemeOptions =>
            {
                // Default
                policySchemeOptions.ForwardDefault = options.DefaultScheme;

                // SchemePolicy
                if (options.AuthenticateSchemePolicy != null) policySchemeOptions.ForwardAuthenticate = PolicyAuthenticationDefaults.AuthenticatePolicyScheme;
                if (options.ChallengeSchemePolicy != null) policySchemeOptions.ForwardChallenge = PolicyAuthenticationDefaults.ChallengePolicyScheme;
                if (options.ForbidSchemePolicy != null) policySchemeOptions.ForwardForbid = PolicyAuthenticationDefaults.ForbidPolicyScheme;
                if (options.SignInSchemePolicy != null) policySchemeOptions.ForwardSignIn = PolicyAuthenticationDefaults.SignInPolicyScheme;
                if (options.SignOutSchemePolicy != null) policySchemeOptions.ForwardSignOut = PolicyAuthenticationDefaults.SignOutPolicyScheme;
            });

            // AuthenticateSchemePolicy
            if (options.AuthenticateSchemePolicy != null)
            {
                builder.AddPolicyScheme(PolicyAuthenticationDefaults.AuthenticatePolicyScheme, null, policySchemeOptions =>
                {
                    // Selector
                    policySchemeOptions.ForwardDefaultSelector = context => options.AuthenticateSchemePolicy(context);
                });
            }

            // ChallengeSchemePolicy
            if (options.ChallengeSchemePolicy != null)
            {
                builder.AddPolicyScheme(PolicyAuthenticationDefaults.ChallengePolicyScheme, null, policySchemeOptions =>
                {
                    // Selector
                    policySchemeOptions.ForwardDefaultSelector = context => options.ChallengeSchemePolicy(context);
                });
            }

            // ForbidSchemePolicy
            if (options.ForbidSchemePolicy != null)
            {
                builder.AddPolicyScheme(PolicyAuthenticationDefaults.ForbidPolicyScheme, null, policySchemeOptions =>
                {
                    // Selector
                    policySchemeOptions.ForwardDefaultSelector = context => options.ForbidSchemePolicy(context);
                });
            }

            // SignInSchemePolicy
            if (options.SignInSchemePolicy != null)
            {
                builder.AddPolicyScheme(PolicyAuthenticationDefaults.SignInPolicyScheme, null, policySchemeOptions =>
                {
                    // Selector
                    policySchemeOptions.ForwardDefaultSelector = context => options.SignInSchemePolicy(context);
                });
            }

            // SignOutSchemePolicy
            if (options.SignOutSchemePolicy != null)
            {
                builder.AddPolicyScheme(PolicyAuthenticationDefaults.SignOutPolicyScheme, null, policySchemeOptions =>
                {
                    // Selector
                    policySchemeOptions.ForwardDefaultSelector = context => options.SignOutSchemePolicy(context);
                });
            }

            // Return
            return builder;
        }
    }
}
