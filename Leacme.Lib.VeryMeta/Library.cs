// Copyright (c) 2017 Leacme (http://leac.me). View LICENSE.md for more information.
using System;
using System.Collections.Generic;
using System.Linq;
using MetadataExtractor;

namespace Leacme.Lib.VeryMeta {

	public class Library {

		public Library() {

		}

		/// <summary>
		/// Retrieves the metadata of the media file if it's valid.
		/// /// </summary>
		/// <param name="directoryName">Grouping of the metadata tags.</param>
		/// <param name="name">Metadata tag name.</param>
		/// <param name="description">Metadata tag value.</param>
		/// <returns>List of file metadata tags.</returns>
		public List<(string directoryName, string name, string description)> GetFileMetadata(Uri file) {
			var mdList = new List<(string, string, string)>();
			var dirs = new List<Directory>();

			try {
				dirs = ImageMetadataReader.ReadMetadata(file.LocalPath).ToList();
			} catch (ImageProcessingException) {
				return mdList;
			}

			foreach (var dir in dirs) {
				foreach (var tag in dir.Tags) {
					mdList.Add((tag.DirectoryName, tag.Name, tag.Description));
				}
			}
			return mdList;
		}
	}
}