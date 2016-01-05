using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
    [TestClass]
    [DeploymentItem("data/citiesTestDataLab4.txt")]
    [DeploymentItem("data/linksTestDataLab4.txt")]
    [DeploymentItem("data/citiesTestDataLab12.txt")]
    [DeploymentItem("data/linksTestDataLab12.txt")]
    public class Lab12Test
    {
        [TestMethod]
        public void TestRouteSerialCorrectness4()
        {
            TestRouteSerialCorrectness("4", 28, 32, 4704, 870, 702, 2552197);
        }

        [TestMethod]
        public void TestRouteSerialCorrectness12()
        {
            TestRouteSerialCorrectness("12", 83, 178, 41334, 7304, 6806, 145904322);
        }

        private void TestRouteSerialCorrectness(string _suffix, int _cities, int _routes, int _resultsA, int _resultsB, int _resultsC, int _hash)
        {
            var cities = new Cities();
            cities.ReadCities("citiesTestDataLab" + _suffix + ".txt");
            Assert.AreEqual(_cities, cities.Count);

            var routes = new Routes(cities);
            int count = routes.ReadRoutes("linksTestDataLab" + _suffix + ".txt");
            Assert.AreEqual(_routes, count);

            List<List<Link>> allRoutesSerial = routes.FindAllShortestRoutes();

            //should return all combinations of routes (cities * routes * TransportModes)
            Assert.AreEqual(_resultsA, allRoutesSerial.Count());

            //filter out non-existing routes
            allRoutesSerial = allRoutesSerial.Where(r => r != null).ToList();

            //should've found a subset of valid routes
            Assert.AreEqual(_resultsB, allRoutesSerial.Count());

            //filter out zero-length routes
            allRoutesSerial = allRoutesSerial.Where(r => r.Count() > 0).ToList();

            //should've found a subset of non-zero-length routes
            Assert.AreEqual(_resultsC, allRoutesSerial.Count());

            //sort both lists in a deterministic fashion
            foreach (var list in new[] { allRoutesSerial })
                list.Sort((a, b) =>
                               string.Join(":", a.Select(l => l.FromCity.Name + " " + l.ToCity.Name + " " + l.TransportMode))
                    .CompareTo(string.Join(":", b.Select(l => l.FromCity.Name + " " + l.ToCity.Name + " " + l.TransportMode))));

            //serialize lists to a string
            var txtSerial = string.Join("/", allRoutesSerial.Select(i => string.Join(":", i.Select(l => l.FromCity.Name + " " + l.ToCity.Name + " " + l.TransportMode))));

            //the algorithm should deliver the expected result
            var hash = txtSerial.Select((ch, index) => ((int)ch) * index).Aggregate((a, b) => a ^ b);
            Assert.AreEqual(hash, _hash);
        }



        [TestMethod]
        public void TestRouteParallelCorrectness4()
        {
            TestRouteParallelCorrectness("4", 28, 32, 4704, 870, 702);
        }

        [TestMethod]
        public void TestRouteParallelCorrectness12()
        {
            TestRouteParallelCorrectness("12", 83, 178, 41334, 7304, 6806);
        }

        private void TestRouteParallelCorrectness(string _suffix, int _cities, int _routes, int _resultsA, int _resultsB, int _resultsC)
        {
            var cities = new Cities();
            cities.ReadCities("citiesTestDataLab" + _suffix + ".txt");
            Assert.AreEqual(_cities, cities.Count);

            var routes = new Routes(cities);
            int count = routes.ReadRoutes("linksTestDataLab" + _suffix + ".txt");
            Assert.AreEqual(_routes, count);

            List<List<Link>> allRoutesSerial = routes.FindAllShortestRoutes();
            List<List<Link>> allRoutesParallel = routes.FindAllShortestRoutesParallel();

            //should return all combinations of routes (cities * routes * TransportModes)
            Assert.AreEqual(_resultsA, allRoutesSerial.Count());
            Assert.AreEqual(_resultsA, allRoutesParallel.Count());

            //filter out non-existing routes
            allRoutesSerial = allRoutesSerial.Where(r => r != null).ToList();
            allRoutesParallel = allRoutesParallel.Where(r => r != null).ToList();

            //should've found a subset of valid routes
            Assert.AreEqual(_resultsB, allRoutesSerial.Count());
            Assert.AreEqual(_resultsB, allRoutesParallel.Count());

            //filter out zero-length routes
            allRoutesSerial = allRoutesSerial.Where(r => r.Count() > 0).ToList();
            allRoutesParallel = allRoutesParallel.Where(r => r.Count() > 0).ToList();

            //should've found a subset of non-zero-length routes
            Assert.AreEqual(_resultsC, allRoutesSerial.Count());
            Assert.AreEqual(_resultsC, allRoutesParallel.Count());

            //sort both lists in a deterministic fashion
            foreach (var list in new[] { allRoutesSerial, allRoutesParallel })
                list.Sort((a, b) =>
                               string.Join(":", a.Select(l => l.FromCity.Name + " " + l.ToCity.Name + " " + l.TransportMode))
                    .CompareTo(string.Join(":", b.Select(l => l.FromCity.Name + " " + l.ToCity.Name + " " + l.TransportMode))));

            //serialize lists to a string
            var txtSerial = string.Join("/", allRoutesSerial.Select(i => string.Join(":", i.Select(l => l.FromCity.Name + " " + l.ToCity.Name + " " + l.TransportMode))));
            var txtParallel = string.Join("/", allRoutesParallel.Select(i => string.Join(":", i.Select(l => l.FromCity.Name + " " + l.ToCity.Name + " " + l.TransportMode))));

            //both algorithms should deliver the same result
            Assert.AreEqual(txtSerial, txtParallel);
        }

        [TestMethod]
        public void TestRouteParallelSpeed()
        {
            var cities = new Cities();
            cities.ReadCities("citiesTestDataLab12.txt");
            var routes = new Routes(cities);
            routes.ReadRoutes("linksTestDataLab12.txt");

            //warmup
            routes.FindAllShortestRoutesParallel();

            //execute and measure time
            var timeA = DateTime.Now;
            List<List<Link>> allRoutesSerial = routes.FindAllShortestRoutes();
            var timeB = DateTime.Now;
            List<List<Link>> allRoutesParallel = routes.FindAllShortestRoutesParallel();
            var timeC = DateTime.Now;
            var oldAffinity = Process.GetCurrentProcess().ProcessorAffinity;
            Process.GetCurrentProcess().ProcessorAffinity = (IntPtr)1;
            routes.FindAllShortestRoutesParallel();
            Process.GetCurrentProcess().ProcessorAffinity = oldAffinity;
            var timeD = DateTime.Now;



            var factor = (timeC - timeB).TotalSeconds / (timeB - timeA).TotalSeconds;
            var slowdown = (timeD - timeC).TotalSeconds / (timeB - timeA).TotalSeconds;

            Trace.WriteLine($"Factor: {factor:F2}, Slowdown: {slowdown:F2}");

            //parallel execution on a single core shouldn't be much slower
            Assert.IsTrue(slowdown < 1.1);

            switch (Environment.ProcessorCount)
            {
                case 1:
                    break;
                case 2:
                    //expect at least 25% reduction with 2 cores
                    Assert.IsTrue(factor < 0.75);
                    break;
                case 3:
                    //expect at least 35% reduction with 3 cores
                    Assert.IsTrue(factor < 0.65);
                    break;
                default:
                    //expect at least 45% reduction with 4+ cores
                    Assert.IsTrue(factor < 0.55);
                    break;
            }
        }
    }
}
