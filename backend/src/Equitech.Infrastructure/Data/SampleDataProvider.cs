using Equitech.Domain.Interfaces;
using Equitech.Domain.Models;

namespace Equitech.Infrastructure.Data;

public class SampleDataProvider : ISampleDataProvider
{
    private readonly List<JobDescriptionDto> _sampleJobs;
    private readonly List<CandidateProfileDto> _sampleCandidates;

    public SampleDataProvider()
    {
        _sampleJobs = new List<JobDescriptionDto>
        {
            new()
            {
                SourceId = "src_internal_001",
                SourceJobId = "JD-DA-2026-01",
                Title = "Data Analyst - Merchant Analytics",
                Employer = "PaySphere Technologies Ltd.",
                CanonicalUrl = "https://careers.paysphere.example.com/jobs/da-01",
                Location = "Mumbai, India (Hybrid)",
                CountryCode = "IND",
                EmploymentType = "Full-time",
                SalaryMin = 700000,
                SalaryMax = 1000000,
                SalaryCurrency = "INR",
                PostedAt = "2026-10-15T09:00:00Z",
                Status = "Verified open",
                Description = "PaySphere is seeking a Data Analyst to join our Merchant Analytics team. You will analyze high-volume payment transaction datasets to identify drop-off trends, build automated KPI dashboards, and support product decision-making.\n\nKey Responsibilities:\n- Write advanced SQL queries (CTEs, Window Functions) on Snowflake/PostgreSQL databases.\n- Design and maintain interactive Power BI dashboards tracking merchant settlement cycles, churn, and transaction success rates.\n- Perform exploratory data analysis using Python (Pandas, NumPy) and basic statistical testing.\n- Collaborate with product managers to formulate A/B test hypotheses and interpret sample size experiments.\n- Build lightweight ETL data transformations using SQL or dbt.\n\nRequired Qualifications:\n- 2+ years of professional experience in data analysis or business intelligence.\n- Mastery of SQL including query optimization and multi-table joins.\n- Demonstrated experience building production dashboards in Power BI.\n- Working knowledge of Python for data manipulation.\n- Strong stakeholder communication skills.\n\nPreferred Qualifications:\n- Prior experience in FinTech, digital payments, or e-commerce.\n- Familiarity with Git version control and dbt (data build tool)."
            },
            new()
            {
                SourceId = "src_internal_002",
                SourceJobId = "JD-EQ-2026-02",
                Title = "Equity Research Associate - Banking & Financial Services",
                Employer = "Apex Global Securities Ltd.",
                CanonicalUrl = "https://careers.apexsecurities.example.com/jobs/eq-02",
                Location = "Mumbai, India (Onsite)",
                CountryCode = "IND",
                EmploymentType = "Full-time",
                SalaryMin = 1200000,
                SalaryMax = 1800000,
                SalaryCurrency = "INR",
                PostedAt = "2026-10-14T08:30:00Z",
                Status = "Verified open",
                Description = "Apex Global Securities is hiring an Equity Research Associate to support our Lead Banking Analyst covering large-cap Indian commercial banks and NBFCs.\n\nKey Responsibilities:\n- Construct comprehensive 3-statement financial models and dynamic forecasting models for covered commercial banks and NBFCs.\n- Conduct discounted cash flow (DCF), Dividend Discount Model (DDM), and residual income valuations.\n- Draft thematic initiation-of-coverage reports, quarterly earnings reviews, and management conference call takeaway notes.\n- Track high-frequency banking metrics: Net Interest Margins (NIM), NPA slippages, Provision Coverage Ratio (PCR), and Capital Adequacy (CRAR).\n- Support institutional client queries and prepare presentation decks for fund managers.\n\nRequired Qualifications:\n- MBA in Finance, Chartered Accountant (CA), or CFA charterholder / Level 2+ candidate.\n- Minimum 2 years of prior experience in equity research, investment banking, or corporate credit evaluation.\n- Demonstrated financial modeling capabilities with strong attention to accounting disclosures.\n- In-depth understanding of RBI regulations and Indian banking balance sheet dynamics.\n\nPreferred Qualifications:\n- Proficiency with Bloomberg, FactSet, or Capital IQ."
            },
            new()
            {
                SourceId = "src_internal_003",
                SourceJobId = "JD-SWE-2026-03",
                Title = "Software Developer - Full Stack (.NET 8 & Vue 3)",
                Employer = "Novus Cloud Software",
                CanonicalUrl = "https://careers.novuscloud.example.com/jobs/swe-03",
                Location = "Bengaluru, India (Remote)",
                CountryCode = "IND",
                EmploymentType = "Full-time",
                SalaryMin = 900000,
                SalaryMax = 1400000,
                SalaryCurrency = "INR",
                PostedAt = "2026-10-16T07:15:00Z",
                Status = "Verified open",
                Description = "Novus Cloud Software builds enterprise workflow automation platforms. We are looking for a Full Stack Software Developer to design and implement resilient REST APIs and reactive modern web UIs.\n\nKey Responsibilities:\n- Develop high-performance backend microservices using ASP.NET Core Web API (.NET 8) and Entity Framework Core.\n- Build single-page application (SPA) frontends using Vue 3 (Composition API), TypeScript, and Pinia.\n- Design relational schemas, write optimized SQL queries, and manage migrations in PostgreSQL.\n- Implement secure authentication flows utilizing OAuth2 / OpenID Connect (OIDC) and JWT.\n- Containerize applications using Docker and write automated unit/integration tests with xUnit and Vitest.\n\nRequired Qualifications:\n- 2-4 years of software development experience with C# / .NET.\n- Hands-on experience developing RESTful APIs and working with relational databases (PostgreSQL or SQL Server).\n- Frontend proficiency with modern JavaScript/TypeScript and a modern reactive framework (Vue, React, or Angular).\n- Solid understanding of OOP principles, Clean Architecture, and dependency injection.\n\nPreferred Qualifications:\n- Prior production experience specifically with Vue 3 and TypeScript.\n- Experience with background task schedulers (Hangfire or RabbitMQ)."
            }
        };

        _sampleCandidates = new List<CandidateProfileDto>
        {
            new()
            {
                ProfileId = "cand_001_aarav",
                FullName = "Aarav Sharma",
                Headline = "Junior BI Analyst | SQL & Excel Specialist",
                Location = "Pune, India",
                Summary = "Data analyst with 1.5 years of experience in retail reporting, building Excel financial summaries and SQL database extractions. Seeking to specialize in FinTech product analytics.",
                EmploymentHistory = new List<CandidateEmploymentDto>
                {
                    new()
                    {
                        Company = "Kalyan Retail Solutions",
                        Role = "Junior Reporting Analyst",
                        StartDate = "2025-05-01",
                        IsCurrent = true,
                        Responsibilities = new List<string>
                        {
                            "Maintained weekly sales reporting across 45 stores using MS Excel (VLOOKUP, Pivot Tables, Power Query).",
                            "Wrote basic SQL queries on PostgreSQL to extract daily inventory and stock movement logs.",
                            "Created an experimental Power BI report for regional store performance."
                        }
                    }
                },
                Education = new List<CandidateEducationDto>
                {
                    new() { Institution = "Pune University", Degree = "Bachelor of Science in Statistics", GraduationYear = 2024 }
                },
                Skills = new List<CandidateSkillDto>
                {
                    new() { Name = "SQL (Basic Joins, Group By)", Proficiency = "Intermediate", Evidence = "Extracted daily inventory logs at Kalyan Retail." },
                    new() { Name = "Microsoft Excel (VLOOKUP, Power Query)", Proficiency = "Advanced", Evidence = "Automated regional sales reports." },
                    new() { Name = "Power BI", Proficiency = "Beginner", Evidence = "Created 1 pilot store report." },
                    new() { Name = "Descriptive Statistics", Proficiency = "Intermediate", Evidence = "B.Sc. in Statistics coursework." }
                },
                Certifications = new List<string>(),
                Achievements = new List<string>
                {
                    "Reduced weekly sales report turnaround time from 6 hours to 2 hours via Power Query automation."
                }
            },
            new()
            {
                ProfileId = "cand_002_meera",
                FullName = "Meera Iyer",
                Headline = "Credit Risk Analyst | CA Finalist | Financial Modeling",
                Location = "Mumbai, India",
                Summary = "Chartered Accountant with 2.5 years of corporate credit risk assessment experience in commercial banking. Skilled in statutory audit, cash flow analysis, and balance sheet stress testing. Transitioning to sell-side equity research.",
                EmploymentHistory = new List<CandidateEmploymentDto>
                {
                    new()
                    {
                        Company = "Vistara Commercial Bank",
                        Role = "Credit Analyst - Corporate Banking",
                        StartDate = "2024-03-01",
                        IsCurrent = true,
                        Responsibilities = new List<string>
                        {
                            "Evaluated working capital and term loan proposals for mid-market manufacturing companies.",
                            "Analyzed audited financial statements, CMA reports, debt service coverage ratios (DSCR), and collateral adequacy.",
                            "Prepared credit approval memorandums presented to the Credit Committee."
                        }
                    },
                    new()
                    {
                        Company = "R.K. Joshi & Associates, CAs",
                        Role = "Articled Assistant",
                        StartDate = "2021-01-15",
                        EndDate = "2024-01-15",
                        IsCurrent = false,
                        Responsibilities = new List<string>
                        {
                            "Conducted statutory and internal audits of public and private sector banks, focusing on NPA identification and provisioning compliance under RBI guidelines."
                        }
                    }
                },
                Education = new List<CandidateEducationDto>
                {
                    new() { Institution = "ICAI", Degree = "Chartered Accountant (CA)", GraduationYear = 2024 },
                    new() { Institution = "CFA Institute", Degree = "CFA Level 1 Passed", GraduationYear = 2025 }
                },
                Skills = new List<CandidateSkillDto>
                {
                    new() { Name = "Financial Statement Analysis", Proficiency = "Advanced", Evidence = "Underwrote 40+ corporate credit loan proposals." },
                    new() { Name = "RBI Banking Regulations & NPA Provisioning", Proficiency = "Advanced", Evidence = "Bank statutory audits at R.K. Joshi." },
                    new() { Name = "Credit Risk & DSCR Modeling", Proficiency = "Intermediate", Evidence = "Built corporate borrower cash flow stress models." },
                    new() { Name = "Financial Modeling in Excel", Proficiency = "Intermediate", Evidence = "Constructed corporate debt repayment schedules." }
                },
                Certifications = new List<string> { "Chartered Accountant (ICAI, 2024)" },
                Achievements = new List<string>
                {
                    "Ranked in top 5% of ICAI CA Final Examination (Nov 2023)."
                }
            },
            new()
            {
                ProfileId = "cand_003_rohan",
                FullName = "Rohan Deshmukh",
                Headline = "Backend C# .NET Developer | ASP.NET Core & SQL Server",
                Location = "Bengaluru, India",
                Summary = "Backend software engineer with 2.5 years of experience building C# REST APIs and enterprise SQL Server databases. Strong in clean code, asynchronous programming, and unit testing. Actively expanding frontend skills in Vue 3.",
                EmploymentHistory = new List<CandidateEmploymentDto>
                {
                    new()
                    {
                        Company = "Trident Tech Enterprises",
                        Role = "Software Engineer",
                        StartDate = "2024-06-01",
                        IsCurrent = true,
                        Responsibilities = new List<string>
                        {
                            "Developed and maintained 12+ RESTful endpoints in ASP.NET Core 7.0 for order processing services.",
                            "Implemented Entity Framework Core code-first migrations and wrote stored procedures in MS SQL Server.",
                            "Authored unit and integration test suites using xUnit and Moq, achieving 82% test coverage."
                        }
                    }
                },
                Education = new List<CandidateEducationDto>
                {
                    new() { Institution = "Visvesvaraya Technological University (VTU)", Degree = "B.E. in Computer Science", GraduationYear = 2023 }
                },
                Skills = new List<CandidateSkillDto>
                {
                    new() { Name = "C# / ASP.NET Core", Proficiency = "Intermediate", Evidence = "2.5 years production API development at Trident Tech." },
                    new() { Name = "Entity Framework Core & SQL Server", Proficiency = "Intermediate", Evidence = "Built relational schemas and LINQ queries." },
                    new() { Name = "xUnit & Moq Testing", Proficiency = "Intermediate", Evidence = "Maintained 82% code coverage." },
                    new() { Name = "JavaScript / HTML / CSS", Proficiency = "Beginner", Evidence = "Maintained internal jQuery tooling." },
                    new() { Name = "Git & GitHub", Proficiency = "Intermediate", Evidence = "Daily collaborative branching workflows." }
                },
                Certifications = new List<string> { "Microsoft Certified: Azure Fundamentals (AZ-900)" },
                Achievements = new List<string>
                {
                    "Optimized order processing API endpoint latency by 40% using asynchronous tasks and EF Core eager loading."
                }
            }
        };
    }

    public IReadOnlyList<JobDescriptionDto> GetSampleJobs() => _sampleJobs;
    public IReadOnlyList<CandidateProfileDto> GetSampleCandidates() => _sampleCandidates;

    public (JobDescriptionDto Job, CandidateProfileDto Candidate)? GetPair(string roleKey)
    {
        return roleKey.ToLowerInvariant() switch
        {
            "data-analyst" or "da" => (_sampleJobs[0], _sampleCandidates[0]),
            "equity-research" or "eq" => (_sampleJobs[1], _sampleCandidates[1]),
            "software-developer" or "swe" => (_sampleJobs[2], _sampleCandidates[2]),
            _ => null
        };
    }
}
