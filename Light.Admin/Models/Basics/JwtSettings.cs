﻿namespace Light.Admin.Mongo.Basics
{
    public class JwtSettings
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }


        public string SecretKey { get; set; }

        public int ExpireSeconds { get; set; }

    }
}
