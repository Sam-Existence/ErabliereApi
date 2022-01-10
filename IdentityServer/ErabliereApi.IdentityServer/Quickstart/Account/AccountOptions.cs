// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;

namespace IdentityServerHost.Quickstart.UI;

public class AccountOptions
{
    public readonly static bool AllowLocalLogin = true;
    public readonly static bool AllowRememberLogin = true;
    public readonly static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);

    public readonly static bool ShowLogoutPrompt = true;
    public readonly static bool AutomaticRedirectAfterSignOut = false;

    public readonly static string InvalidCredentialsErrorMessage = "Nom d'utilisateur ou mot de passe invalide";
}
