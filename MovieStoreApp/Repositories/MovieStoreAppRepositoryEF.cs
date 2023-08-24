using MovieStoreApp.Models;
using MovieStoreApp.ModelsDTO;
using MovieStoreApp.ModelsVM;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http;
using Newtonsoft.Json;

namespace MovieStoreApp.Repositories
{
    public class MovieStoreAppRepositoryEF
    {
        MoviesAppDbContext _context;

        public MovieStoreAppRepositoryEF(MoviesAppDbContext context)
        {
            _context = context;
        }

        public bool AddNewCustomerAndUser(NewCustomerDTO newCustomer)
        {
            Customer cust = new Customer()
            {
                FirstName = newCustomer.FirstName,
                LastName = newCustomer.LastName,
                StreetAddress = newCustomer.StreetAddress,
                City = newCustomer.City,
                State = newCustomer.State,
                Telephone = newCustomer.Telephone,
            };

            User unew = new User
            {
                UserEmail = newCustomer.Email,
                Password = newCustomer.Password,
            };
            unew.Users.Add(cust);
            _context.Users.Add(unew);
            _context.SaveChanges();
            return true;
        }

        public UserInfo VerifyLogin(User user)
        {
            UserInfo userInfo = null;
            var usr = _context.Users.Include(u => u.Users).Where(u => u.UserEmail == user.UserEmail & u.Password == user.Password).FirstOrDefault<User>();
            if (usr != null)
            {
                userInfo = new UserInfo()
                {
                    Email = usr.UserEmail,
                    UserId = usr.Users.ToList()[0].UserId
                };
            }

            return userInfo;
        }

        public async Task<bool> UpdatePassword(string userEmail, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == userEmail);

            if (user != null)
            {
                user.Password = newPassword;
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> CheckEmailExists(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == email);
            return user != null;
        }

        public bool AddMovieAndPicture(Movie prod, byte[] bdata)
        {
            _context.Add(prod);
            _context.SaveChanges();
            MoviePicture ppic = new MoviePicture
            {
                Picture = bdata,
                PictureDate = DateTime.Now,
                MovieId = prod.MovieId
            };
            _context.Add(ppic);
            _context.SaveChanges();

            return true;
        }

        public bool AddMoviePicture(MoviePicture mpic)
        {
            _context.Add(mpic);
            _context.SaveChanges();
            return true;
        }

        public bool AddMovieGenre(Genre movGenre)
        {

            Genre gen = new Genre
            {
                GenreId = movGenre.GenreId,
                GenreName = movGenre.GenreName,
            };

            _context.Genres.Add(gen);
            _context.SaveChanges();
            return true;
        }

        public bool AddBulkMovie(Movie prod)
        {


            // Check if the movie already exists in the database based on MovieId or ImdbId
            bool movieExists = _context.Movies.Any(m => m.MovieId == prod.MovieId || m.ImdbId == prod.ImdbId);

            if (movieExists)
            {
                // Movie already exists, return false to indicate failure
                return false;
            }

            // Movie doesn't exist, add it to the database
            _context.Movies.Add(prod);
            _context.SaveChanges();

            // Movie added successfully, return true to indicate success
            return true;

        }


        public List<MovieDTO> GetMoviesByGenre(int genid)
        {
            var movies = (_context.Movies.Include(m => m.MoviePictures).Where(m => m.GenreId == genid).Select(m =>
            new MovieDTO
            {
                MovieId = m.MovieId,
                Title = m.Title,
                OutOfStock = m.OutOfStock,
                OnSale = m.OnSale,
                Price = m.Price,
                MovieImage = m.MoviePictures != null ?
                (m.MoviePictures.Count > 0 ? GetImageFromBytes(m.MoviePictures.ToList()[0].Picture) : null) : null
            })).ToList();
            return movies;
        }

        public MovieDTO GetMovieById(int movid)
        {
            var movie = (_context.Movies.Include(m => m.MoviePictures).Where(m => m.MovieId == movid).Select(m =>
            new MovieDTO
            {
                MovieId = m.MovieId,
                Title = m.Title,
                OutOfStock = m.OutOfStock,
                OnSale = m.OnSale,
                Price = m.Price,
                ReleasedYear = m.ReleasedYear,
                MovieImage = m.MoviePictures != null ?
                (m.MoviePictures.Count > 0 ? GetImageFromBytes(m.MoviePictures.ToList()[0].Picture) : null) : null
            })).FirstOrDefault<MovieDTO>();
            return movie;
        }

        //public MovieDTO GetMovieById(int movid)

        //{
        //    var movie = (_context.Movies.Include(m => m.MoviePictures)
        //                .Where(m => m.MovieId == movid)
        //                .Select(m => new MovieDTO
        //                {
        //                    MovieId = m.MovieId,
        //                    Title = m.Title,
        //                    OutOfStock = m.OutOfStock,
        //                    OnSale = m.OnSale,
        //                    Price = m.Price,
        //                    ReleasedYear = m.ReleasedYear,
        //                    MovieImage = GetMovieImageFromTMDb(m.Title) // fetch image from TMDB
        //                }))
        //                .FirstOrDefault<MovieDTO>();
        //    return movie;
        //}

        public async Task<string> GetMovieImageFromTMDbAsync(string title)
        {
            string apiKey = "4d211759108373f2af45a56063922d02";
            string apiUrl = $"https://api.themoviedb.org/3/search/movie?api_key={apiKey}&query={Uri.EscapeDataString(title)}";

            using (HttpClient client = new HttpClient())
            {
                var json = await client.GetStringAsync(apiUrl);
                var response = JsonConvert.DeserializeObject<dynamic>(json);

                if (response.results != null && response.results.Count > 0)
                {
                    string posterPath = response.results[0].poster_path;
                    if (!string.IsNullOrEmpty(posterPath))
                    {
                        return $"https://image.tmdb.org/t/p/w500{posterPath}";
                    }
                }
            }

            return null;
        }



        protected static string GetImageFromBytes(byte[]? image)
        {
            if (image != null)
            {
                //return $"MovieImage/png;base64,{Convert.ToBase64String(image)}";
                return $"data:image/png;base64, {Convert.ToBase64String(image)}";
            }
            else
            {
                return string.Empty;
            }
        }

        public List<MovieDTO> GetAllMovies()
        {
            // Retrieve all movies from the database
            var movies = _context.Movies.Select(m => new MovieDTO
            {
                MovieId = m.MovieId,
                Title = m.Title,
                Price = m.Price,
                OnSale = m.OnSale,
                OutOfStock = m.OutOfStock,
                MovieImage = m.MoviePictures != null && m.MoviePictures.Count > 0
            ? GetImageFromBytes(m.MoviePictures.ToList()[0].Picture)
            : null
            }).ToList();

            return movies;
        }

        public bool PlaceOrder(List<CartItem> CList, UserInfo uinfo)
        {
            CustomerOrder co = new CustomerOrder { UserId = uinfo.UserId, OrderDate = DateTime.Now };
            _context.CustomerOrders.Add(co);
            _context.SaveChanges();
            foreach (var item in CList)
            {
                Order order = new Order()
                {
                    OrderId = co.OrderId,
                    MovieId = item.MovieId,
                    Quantity = item.Quantity

                };
                _context.Orders.Add(order);
            }
            _context.SaveChanges();
            return true;

        }

        public List<OrderDTO> GetAllOrders()
        {
            List<OrderDTO> orders;
            orders = (_context.CustomerOrders.Select(o =>
            new OrderDTO
            {

                UserId = o.UserId,
                FirstName = o.User.FirstName,
                LastName = o.User.LastName,
                OrderDate = o.OrderDate,
                OrderID = o.OrderId,
                Telephone = o.User.Telephone,
                Address = o.User.StreetAddress,
                City = o.User.City,
                State = o.User.State
            })).ToList();

            return orders;
        }

        public OrderDTO GetOrderInfo(int orderId)
        {
            var order = (_context.CustomerOrders.Where(o => o.OrderId == orderId)
                .Select(o =>
                new OrderDTO
                {
                    UserId = o.UserId,
                    FirstName = o.User.FirstName,
                    LastName = o.User.LastName,
                    OrderDate = o.OrderDate,
                    OrderID = o.OrderId,
                    Telephone = o.User.Telephone,
                    Address = o.User.StreetAddress,
                    City = o.User.City,
                    State = o.User.State
                })).FirstOrDefault();

            return order;
        }

        public List<OrderItemDTO> GetOrderItems(int orderId)
        {
            List<OrderItemDTO> orders;
            orders = (from o in _context.Orders
                      join m in _context.Movies on o.MovieId equals m.MovieId
                      join co in _context.CustomerOrders on o.OrderId equals co.OrderId
                      where o.OrderId == orderId
                      select new OrderItemDTO
                      {
                          MovieId = o.MovieId,
                          Title = m.Title,
                          Quantity = o.Quantity,
                          OrderDate = co.OrderDate.ToShortDateString(),
                          OrderId = o.OrderId,
                          Price = m.Price
                      }).ToList();

            return orders;
        }

        public async Task<List<Movie>> GetMostPopularMoviesAsync(int top = 10)
        {
            return await _context.Movies
                .GroupJoin(
                    _context.CustomerRatings,
                    movie => movie.MovieId,
                    rating => rating.MovieId,
                    (movie, rating) => new { Movie = movie, Rating = rating }
                )
                .Select(group => new
                {
                    Movie = group.Movie,
                    AverageRating = group.Rating.Average(rating => rating.Rating)
                })
                .OrderByDescending(group => group.AverageRating)
                .Take(top)
                .Select(group => group.Movie)
                .ToListAsync();
        }

        public async Task AddBulkCustomerRatings(List<CustomerRating> customerRatings)
        {
            await _context.CustomerRatings.AddRangeAsync(customerRatings);
            await _context.SaveChangesAsync();
        }

        public List<string> GetMoviePictureBase64Strings()
        {
            List<string> base64Strings = _context.MoviePictures
                .Select(mp => Convert.ToBase64String(mp.Picture))
                .ToList();
            return base64Strings;
        }

        public async Task<bool> SaveMoviePoster(int movieId, byte[] posterBytes)
        {
            try
            {
                MoviePicture newPic = new MoviePicture
                {
                    MovieId = movieId,
                    Picture = posterBytes,
                    PictureDate = DateTime.Now
                };

                _context.MoviePictures.Add(newPic);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                // Log or handle exceptions as required
                return false;
            }

        }

        public Movie GetMovieByTmdbId(int tmdbId)
        {
            return _context.Movies.FirstOrDefault(m => m.TmdbId == tmdbId);
        }

        public async Task<int?> GetTmdbIdByMovieId(int movieId)
        {
            var tmdbId = await _context.Movies
            .Where(m => m.MovieId == movieId)
            .Select(m => m.TmdbId)
            .FirstOrDefaultAsync();

            return (tmdbId != 0 ? tmdbId : (int?)null);
        }
    }
    


}
