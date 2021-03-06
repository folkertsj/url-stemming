﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UrlStemming.Core;

namespace Tests
{
    [TestClass]
    public class Core
    {
        [TestMethod]
        public void PassThrough()
        {
            // There should be no change here
            var url = "http://gadgetopia.com/";
            Assert.AreEqual(url, UrlStemmer.Stem(url));
        }

        [TestMethod]
        public void ForceHost()
        {
            UrlStemmer.Settings.ForceHost = "gadgetopia.com";

            var url = "http://www.gadgetopia.com/";
            Assert.AreEqual("http://gadgetopia.com/", UrlStemmer.Stem(url));
        }

        [TestMethod]
        public void ForceCase()
        {
            UrlStemmer.Settings.ForceLowerCase = true;

            var url = "http://gadgetopia.com/FOO";
            Assert.AreEqual("http://gadgetopia.com/foo", UrlStemmer.Stem(url));
        }

        [TestMethod]
        public void ForceScheme()
        {
            var url = "http://gadgetopia.com/";

            UrlStemmer.Settings.ForceScheme = "https";
            UrlStemmer.Settings.ForcePort = -1; // Removes the port, since it would still be "80"
            Assert.AreEqual("https://gadgetopia.com/", UrlStemmer.Stem(url));
        }

        [TestMethod]
        public void ReorderQuerystringArgs()
        {
            UrlStemmer.Settings.ReorderQuerystringArguments = true;

            var url = "http://www.gadgetopia.com/foo?c=d&a=b";
            Assert.AreEqual("http://www.gadgetopia.com/foo?a=b&c=d", UrlStemmer.Stem(url));
        }

        [TestMethod]
        public void TrailingSlashs()
        {
            var urlWithoutSlash = "http://www.gadgetopia.com/foo";
            var urlWithSlash = string.Concat(urlWithoutSlash, "/");

            UrlStemmer.Settings.TrailingSlashes = TrailingSlashes.AlwaysStrip;
            Assert.AreEqual(urlWithoutSlash, UrlStemmer.Stem(urlWithSlash));

            UrlStemmer.Settings.TrailingSlashes = TrailingSlashes.AlwaysAdd;
            Assert.AreEqual(urlWithSlash, UrlStemmer.Stem(urlWithoutSlash));

            UrlStemmer.Settings.TrailingSlashes = TrailingSlashes.Ignore;
            Assert.AreEqual(urlWithSlash, UrlStemmer.Stem(urlWithSlash));
            Assert.AreEqual(urlWithoutSlash, UrlStemmer.Stem(urlWithoutSlash));
        }

        [TestMethod]
        public void ArgumentWhitelist()
        {
            UrlStemmer.Settings.ArgumentWhitelist.Add("a");

            var url = "http://gadgetopia.com/?a=b&c=d";
            Assert.AreEqual("http://gadgetopia.com/?a=b", UrlStemmer.Stem(url));
        }

        [TestMethod]
        public void ArgumentBlacklist()
        {
            UrlStemmer.Settings.ArgumentBlacklist.Add("a");

            var url = "http://gadgetopia.com/?a=b&c=d";
            Assert.AreEqual("http://gadgetopia.com/?c=d", UrlStemmer.Stem(url));
        }

        [TestMethod]
        public void RemoveBookmark()
        {
            UrlStemmer.Settings.RemoveBookmarks = true;

            var url = "http://gadgetopia.com/#bookmark";
            Assert.AreEqual("http://gadgetopia.com/", UrlStemmer.Stem(url));
        }

        [TestMethod]
        public void RemoveSubdomain()
        {
            UrlStemmer.Settings.RemoveSubdomain = true;

            var urlWithSubdomain = "http://www.gadgetopia.com/";
            var urlWithoutSubdomain = "http://gadgetopia.com/";
            var localUrl = "http://gadgetopia/";

            Assert.AreEqual("http://gadgetopia.com/", UrlStemmer.Stem(urlWithSubdomain));
            Assert.AreEqual("http://gadgetopia.com/", UrlStemmer.Stem(urlWithoutSubdomain));
            Assert.AreEqual("http://gadgetopia/", UrlStemmer.Stem(localUrl));
        }

        [TestMethod]
        public void DefaultHost()
        {
            var url = "/my/path";
            Assert.AreEqual("http://example.com/my/path", UrlStemmer.Stem(url));
        }

        [TestMethod]
        public void DefaultScheme()
        {
            var url = "gadgetopia.com";
            Assert.AreEqual("http://gadgetopia.com/", UrlStemmer.Stem(url));
        }

		[TestMethod]
		public void ClearQuerystring()
		{
			var url = "http://gadgetopia.com?a=b";
			UrlStemmer.Settings.ClearQuerystring = true;
			Assert.AreEqual("http://gadgetopia.com/", UrlStemmer.Stem(url));
		}

        [TestMethod]
        public void Forceport()
        {
            var url = "http://gadgetopia.com:8080";

            // Forcing to 80 should remove it
            UrlStemmer.Settings.ForcePort = 80;
            Assert.AreEqual("http://gadgetopia.com/", UrlStemmer.Stem(url));

            // Forcing to anything else should change it
            UrlStemmer.Settings.ForcePort = 1234;
            Assert.AreEqual("http://gadgetopia.com:1234/", UrlStemmer.Stem(url));

            // Forcing to something when original has no port, should add it
            url = "http://gadgetopia.com";
            UrlStemmer.Settings.ForcePort = 6789;
            Assert.AreEqual("http://gadgetopia.com:6789/", UrlStemmer.Stem(url));

            // Not forcing, should leave it alone
            url = "http://gadgetopia.com:8080";
            UrlStemmer.Settings.ForcePort = 0;
            Assert.AreEqual("http://gadgetopia.com:8080/", UrlStemmer.Stem(url));

            // Setting to -1 should remove it
            url = "http://gadgetopia.com:8080";
            UrlStemmer.Settings.ForcePort = -1;
            Assert.AreEqual("http://gadgetopia.com/", UrlStemmer.Stem(url));
        }


        [TestInitialize]
        public void TestInitialize()
        {
            UrlStemmer.Reset();
        }

    }
}
