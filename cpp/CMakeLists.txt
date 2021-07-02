cmake_minimum_required(VERSION 3.15)
project(Platform.Converters)
include(${CMAKE_BINARY_DIR}/conanbuildinfo.cmake)
conan_basic_setup(TARGETS)

add_library(${PROJECT_NAME}.Library INTERFACE)
target_include_directories(${PROJECT_NAME}.Library INTERFACE ${PROJECT_NAME})

add_executable(${PROJECT_NAME}.Tests ${PROJECT_NAME}.Tests/${PROJECT_NAME}.Tests.cpp)
target_link_libraries(${PROJECT_NAME}.Tests PRIVATE ${PROJECT_NAME}.Library)
target_link_libraries(${PROJECT_NAME}.Tests PRIVATE CONAN_PKG::gtest)
set_target_properties(${PROJECT_NAME}.Tests PROPERTIES CXX_STANDARD 20)