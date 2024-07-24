CREATE TABLE service (
  [service_id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
  [name] NVARCHAR(32) NOT NULL
);

CREATE TABLE total (
  [total_id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
  [service_id] UNIQUEIDENTIFIER FOREIGN KEY REFERENCES service(service_id),
  [count] INT,
  [timestamp] DATETIME
);

CREATE TABLE profile (
  [profile_id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
  [identifier] NVARCHAR(32) NOT NULL,
  [first_name] NVARCHAR(64) NOT NULL,
  [middle_name] NVARCHAR(64),
  [last_name] NVARCHAR(64) NOT NULL,
  [gender] NVARCHAR(1) NOT NULL,
  [email] NVARCHAR(128) NOT NULL,
  [birthdate] DATE NOT NULL,
  [status] BIT NOT NULL,
  [claimed] BIT,
  [locked] BIT,
  [mfa_method] NVARCHAR(8)
);

CREATE TABLE link (
  [link_id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
  [profile_id] UNIQUEIDENTIFIER FOREIGN KEY REFERENCES profile(profile_id),
  [service_id] UNIQUEIDENTIFIER FOREIGN KEY REFERENCES service(service_id),
  [service_identifier] NVARCHAR(50) NOT NULL,
  [first_name] NVARCHAR(50) NOT NULL,
  [last_name] NVARCHAR(50) NOT NULL,
  [address] NVARCHAR(50),
  [created_date] DATETIME NOT NULL,
  [active] BIT NOT NULL,
  [org_unit_path] NVARCHAR(100) NOT NULL,
  [photo_url] NVARCHAR(128),
  [phone] NVARCHAR(10),
  [organization] NVARCHAR(100),
  [last_activity] DATETIME
);

CREATE TABLE agent (
  [agent_id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
  [username] NVARCHAR(32) NOT NULL,
  [password] NVARCHAR(32) NOT NULL
);

CREATE TABLE job (
  [job_id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
  [service_id] UNIQUEIDENTIFIER FOREIGN KEY REFERENCES service(service_id),
  [start_date] DATETIME NOT NULL,
  [next_runtime] DATETIME NOT NULL,
  [frequency] NVARCHAR(10) NOT NULL,
  [active] BIT NOT NULL
);

CREATE TABLE task (
  [task_id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
  [job_id] UNIQUEIDENTIFIER FOREIGN KEY REFERENCES job(job_id),
  [start_time] DATETIME NOT NULL,
  [end_time] DATETIME NOT NULL,
  [notes] NVARCHAR(100) NOT NULL,
  [active] BIT NOT NULL
);

SELECT * FROM [task]