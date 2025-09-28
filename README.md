# XtraMilesTest
TASK #1: Data Structure Problem Solving
You have 1 million patient records in a relational database. The application must provide
“type-ahead” search on first, last or full names and return matching results in under 100 ms.
1. Which data structure and matching algorithm would you choose for this
autocomplete use-case, and why?
=> If I had to build this autocomplete feature on <strong>SQL Server</strong> with a million patient records, here’s how I’d approach it, 

I’d rely on <strong>non-clustered indexes</strong> on the name columns (first, last, and full name). With that in place, a simple LIKE 'Jo%' query can use the index and quickly narrow down results. For more advanced scenarios, SQL Server also supports Full-Text Search. With that, I could use prefix searches like CONTAINS(FirstName, '"Jo*"'), which is optimized for word-based lookups. Both approaches are essentially doing prefix matching, which is exactly what we need for type-ahead.


2. What is the expected performance of your solution, both in terms of algorithmic time
complexity and real-world query latency?
=> With a non-clustered index, the lookup is basically O(log N) to find the starting point in the index, and then it just streams out matching rows until it hits the result limit. That’s very fast. In my experience, on a table with a million rows, you’d usually see query times in the 5–20 ms range, well below the 100 ms target, as long as the index is properly tuned. Full-Text search tends to be in the same ballpark, maybe 10–30 ms, and it gives more flexibility if you ever need fuzzy matching or multiple language support.


3. What specific tools or technologies would you leverage to implement and optimize
this design?
=> To make this efficient, I’d do a few things:
 - Add non-clustered indexes on FirstName, LastName, and a precomputed FullName.
 - Use TOP N in the query (e.g., TOP 20) so the database doesn’t try to scan too many matches.
 - If needed, enable Full-Text indexing for better scalability on more complex searches.
 - Optionally, I’d add a cache layer (like Redis) for very common prefixes so the DB isn’t hit every time someone types “Jo” or “Ma.”.

 In best practice if we focus with performance, I’d keep SQL Server as the source of truth but sync patient names into Elasticsearch, either with a cron job or real-time CDC. That way, autocomplete queries go straight to Elasticsearch, which is optimized for prefix search and responds in just a few milliseconds. SQL Server handles transactions, Elasticsearch handles fast search — so we stay well under 100 ms. 

 # Practical Test
 its deployed at GCP Compute Engine. with link below.
 [weather-app](https://weather-app.devinfosekitar.my.id/)