﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AspNetCore.Options.Validation.Practice
{
    /// <summary>
    /// 通过IValidateOptions 方式进行验证
    /// </summary>
    public class MyConfigOptions
    {
        public const string MyConfig = "MyConfig";

        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$")]
        public string Key1 { get; set; }

        [Range(0, 1000, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Key2 { get; set; }

        public int Key3 { get; set; }
    }
    public class MyConfigValidation : IValidateOptions<MyConfigOptions>
    {
        public MyConfigOptions _config { get; private set; }

        public MyConfigValidation(IConfiguration config)
        {
            _config = config.GetSection(MyConfigOptions.MyConfig).Get<MyConfigOptions>();
        }

        public ValidateOptionsResult Validate(string name, MyConfigOptions options)
        {
            string vor = null;
            var rx = new Regex(@"^[a-zA-Z''-'\s]{1,40}$");
            var match = rx.Match(options.Key1);

            if (string.IsNullOrEmpty(match.Value))
            {
                vor = $"{options.Key1} doesn't match RegEx \n";
            }
            if (options.Key2 < 0 || options.Key2 > 1000)
            {
                vor = $"{options.Key2} doesn't match Range 0 - 1000 \n";
            }
            if (_config.Key2 != default)
            {
                if (_config.Key3 <= _config.Key2)
                {
                    vor += "Key3 must be > than Key2.";
                }
            }
            if (vor != null)
            {
                //失败时返回的错误信息
                return ValidateOptionsResult.Fail(vor);
            }

            return ValidateOptionsResult.Success;
        }
    }


}
