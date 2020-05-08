# login-and-registration-system
C# Login and Registration System with .NET Framework

Contains a login and registration application with hash/salt built using C#,mysql and .NET Framework. Allows the user to perform CRUD operations on student. 
All database configuration can be found in DbHandler.cs class,

        // --- mysql Connection ---
        static string connString = "datasource=127.0.0.1;port=3306;username=root;password=;database=studentrecords;";
        MySqlConnection conn = new MySqlConnection(connString);

## Database Structure


>###### Database: `studentrecords`
>###### Table structure for table `faculty`
>
>````mysql
> CREATE TABLE `faculty` (
>     `id` int(11) NOT NULL,
>     `facultyname` varchar(255) NOT NULL
> ) ENGINE=InnoDB DEFAULT CHARSET=latin1;
> ````
>
>
>
>###### Table structure for table `student`
>
>```mysql
> CREATE TABLE `student` (
>  `id` int(10) NOT NULL,
>  `indexno` varchar(9) NOT NULL,
>  `firstname` varchar(255) NOT NULL,
>  `lastname` varchar(255) NOT NULL,
>  `address` varchar(255) NOT NULL,
>  `gender` varchar(1) NOT NULL,
>  `dob` date NOT NULL,
>  `email` varchar(255) NOT NULL,
>  `faculty` varchar(10) NOT NULL,
>  `mobile` varchar(10) NOT NULL,
>  `password` varchar(65) NOT NULL,
>  `salt` varchar(256) NOT NULL,
>  `image` blob NOT NULL
> ) ENGINE=InnoDB DEFAULT CHARSET=latin1;
>```
>
>
>
>###### Indexes for dumped tables
>
>###### Indexes for table `faculty`
>
>```mysql
> ALTER TABLE `faculty` ADD PRIMARY KEY (`id`);
>```
>
>
>
>###### Indexes for table `student`
>
>```mysql
> ALTER TABLE `student`
> ADD PRIMARY KEY (`id`),
> ADD KEY `faculty` (`faculty`);
>```
>
>
>
>###### AUTO_INCREMENT for dumped tables
>
>
> ###### AUTO_INCREMENT for table `faculty`
>
>```mysql
> ALTER TABLE `faculty`
>     MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;
>```
>
>
>
>###### AUTO_INCREMENT for table `student`
>
> ```mysql
> ALTER TABLE `student`
>  MODIFY `id` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=26;
>  /*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
>  /*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
>  /*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
>```


