// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace LYM.OAuth2OpenId.IdrServices.Consent
{
    public class ConsentOptions
    {
        public static bool EnableOfflineAccess = true;
        public static string OfflineAccessDisplayName = "Offline Access";
        public static string OfflineAccessDescription = "在脱机状态下也可以访问应用程序和资源";

        public static readonly string MustChooseOneErrorMessage = "您必须至少选择一个权限";
        public static readonly string InvalidSelectionErrorMessage = "无效的选择";
    }
}
