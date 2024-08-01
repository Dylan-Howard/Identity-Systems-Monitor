
-- SELECT *
-- FROM [link] l
-- INNER JOIN [enrollment] e ON e.user_sourced_id = l.link_id
-- INNER JOIN [class] c ON c.sourced_id = e.class_sourced_id

-- SELECT
--   [COLUMN_NAME],
--   [IS_NULLABLE],
--   [DATA_TYPE],
--   [CHARACTER_MAXIMUM_LENGTH]
-- FROM INFORMATION_SCHEMA.COLUMNS
-- WHERE TABLE_NAME = 'enrollment'


-- @TODO: Remove duplicate linked accounts
WITH duplicates AS (
  SELECT
    MAX([link_id]) AS [link_id],
    [profile_id],
    [service_id],
    [service_identifier],
    COUNT([profile_id]) AS [count]
  FROM [link]
  GROUP BY
    [profile_id],
    [service_id],
    [service_identifier]
  HAVING COUNT([profile_id]) > 1
)

SELECT *
FROM duplicates dup
INNER JOIN [service] srv ON srv.service_id = dup.service_id

-- DELETE FROM [link]
-- WHERE [link_id] IN (SELECT [link_id] FROM duplicates)


SELECT
  [class_sourced_id],
  COUNT([class_sourced_id])
FROM [enrollment]
GROUP BY [class_sourced_id]
HAVING COUNT([class_sourced_id]) > 2