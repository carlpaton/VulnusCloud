using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Business.CoordinatePart.Interface;
using Business.Model;
using Common.Serialization.Interface;
using Microsoft.AspNetCore.Http;

namespace Business.CoordinatePart
{
    // TODO - this class is pretty brittle, investigate if the logic can be made better
    /* Example data that needs to be read
      
dependencies": {
    "@angular/animations": "^7.2.1",
    "@angular/common": "^7.2.1",
    "@angular/compiler": "^7.2.1",
    "@angular/compiler-cli": "^7.2.1",
    "@angular/core": "^7.2.1",
    "@angular/forms": "^7.2.1",
    "@angular/http": "^7.2.1",

     
     */

    /// <summary>
    /// Reads components from NPM (Node Package Manager)
    /// Example: `[project path]\packages\[project name]\web\client\package.json`
    /// </summary>
    public class ByPackageJson : ICoordinateParts
    {
        public List<CoordinatePartsModel> GetCoordinateParts(IJsonConvertService jsonConvertService, string type, IFormFile postedFile)
        {
            var coordinateParts = new List<CoordinatePartsModel>();

            using (var memoryStream = new MemoryStream())
            {
                postedFile.CopyTo(memoryStream);
                var stringData = System.Text.Encoding.ASCII.GetString(memoryStream.ToArray());

                var startPos = stringData.IndexOf("dependencies");
                stringData = stringData.Substring(startPos, stringData.Length - startPos);

                var arrayStringData = stringData.Split(Environment.NewLine);
                foreach (var dependency in arrayStringData.Skip(1).ToArray())
                {
                    if (dependency.Contains("}"))
                        break;

                    var lineData = dependency.Split(":");
                    var name = CleanString(lineData[0]);
                    var version = CleanString(lineData[1]);

                    coordinateParts.Add(new CoordinatePartsModel()
                    {
                        Name = name,
                        Namespace = "npm",
                        Type = type,
                        Version = version
                    });
                }
            }

            return coordinateParts;
        }

        private string CleanString(string s)
        {
            return s
                .Trim()
                .Replace("\"", "")
                .Replace("@", "")
                .Replace("^", "")
                .Replace(",", "");
        }
    }
}
