


using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using Newtonsoft.Json;
using TaskManagement.Controllers;
using TaskManagement.DTOs;
using TaskManagement.Entity;
using TaskManagement.Interfaces;
using Xunit;

namespace TaskManagement.Test.Controller{

    public class UserControllerTest{

        private readonly Mock<IUserService> mockUserService;
        private readonly UserController userController;

        public UserControllerTest(){
            this.mockUserService = new Mock<IUserService>();
            this.userController = new UserController(mockUserService.Object);
        }

        [Fact]
        public async Task FindAllUserSuccessfully(){
            
            var paginationDto = new PaginationUserDto 
            { 
                Limit = 10,
                Offset = 0,
                Email = "",
                OrderBy = "asc"
            };

            //Mock data Result

            var role = new Role{
                Name = "Admin",
                Description = "Description",
                Permissions = []
            };

            var DataExpected = new List<object>{
                new User {
                    Id = 1,
                    Name = "Victor",
                    Email = "Test1@gmail.com",
                    Password = "mySecretPassword",
                    role = role
                }
            };

            mockUserService
                .Setup(service => service.findAll(It.IsAny<PaginationUserDto>()))
                    .ReturnsAsync(DataExpected);
            
            //Call method controller
            var result = await userController.finAllUser(paginationDto);


            //Asserts
            var okResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode); //Status is correct

            var responseData = okResult.Value as dynamic;
            Assert.NotNull(responseData);// not null

            var responseList = responseData?.data as IEnumerable<dynamic>;
            Assert.NotNull(responseList);
            Assert.Equal(DataExpected.Count, responseList.Count());
        
            for (int i = 0; i < DataExpected.Count; i++){
                var expectedUser = DataExpected[i] as User; 
                var actualUser = responseList.ElementAt(i);

                Assert.Equal(expectedUser?.Id, actualUser.Id);
                Assert.Equal(expectedUser?.Name, actualUser.Name);
                Assert.Equal(expectedUser?.Email, actualUser.Email);
                Assert.Equal(expectedUser?.Password, actualUser.Password);
        
                // Role
                Assert.Equal(expectedUser?.role.Id, actualUser.role.Id);
                Assert.Equal(expectedUser?.role.Name, actualUser.role.Name);
                Assert.Equal(expectedUser?.role.Description, actualUser.role.Description);
                Assert.Equal(expectedUser?.role.Permissions.Count, actualUser.role.Permissions.Count);
            }
        }

        [Fact]
        public async Task FinAllUserUnexpectedError(){
            // Arrange
            var paginationDto = new PaginationUserDto{
                Limit = 10,
                Offset = 0,
                Email = "",
                OrderBy = "asc"
            };

            mockUserService
                .Setup(service => service.findAll(It.IsAny<PaginationUserDto>()))
                .ThrowsAsync(new UnexpectedErrorException("Unexpected Error"));

            // Act & Assert
            await Assert.ThrowsAsync<UnexpectedErrorException>(
                async () => await userController.finAllUser(paginationDto)
            );
        }
    }
}