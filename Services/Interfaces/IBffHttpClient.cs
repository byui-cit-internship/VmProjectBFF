﻿using vmProjectBFF.DTO;

namespace vmProjectBFF.Services
{
    public interface IBffHttpClient
    {
        public BffResponse Delete(
            string path,
            dynamic content);

        public BffResponse Get(string path);

        public BffResponse Get(
            string path,
            object queryParams);

        public BffResponse Post(
            string path,
            dynamic content);

        public BffResponse Put(
            string path,
            dynamic content);
    }
}
