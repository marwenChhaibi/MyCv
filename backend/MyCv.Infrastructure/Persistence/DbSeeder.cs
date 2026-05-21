using MyCv.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MyCv.Infrastructure.Persistence;

public class DbSeeder(IServiceProvider services, ILogger<DbSeeder> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken ct)
    {
        await using var scope = services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.EnsureCreatedAsync(ct);

        if (!await db.Profiles.AnyAsync(ct)) await SeedProfileAsync(db, ct);
        if (!await db.Experiences.AnyAsync(ct)) await SeedExperiencesAsync(db, ct);
        if (!await db.Skills.AnyAsync(ct)) await SeedSkillsAsync(db, ct);
        if (!await db.Projects.AnyAsync(ct)) await SeedProjectsAsync(db, ct);

        await db.SaveChangesAsync(ct);
        logger.LogInformation("Database seeded.");
    }

    private static Task SeedProfileAsync(AppDbContext db, CancellationToken ct)
    {
        db.Profiles.Add(new ProfileInfo
        {
            Id = Guid.NewGuid(),
            FullName = "Marwen Chhaibi",
            Title = "Senior .NET Developer · Tech Lead",
            TitleFr = "Développeur .NET Senior · Référent Technique",
            Bio = "Senior .NET developer with 7+ years of experience building SaaS platforms and distributed systems. Focused on Clean Architecture, CQRS, and DevOps automation. Open to Tech Lead or senior freelance missions in Paris.",
            BioFr = "Développeur .NET Senior avec plus de 7 ans d'expérience dans la conception de plateformes SaaS et de systèmes distribués. Passionné par la Clean Architecture, le CQRS et l'automatisation CI/CD. À l'écoute d'opportunités Tech Lead ou missions freelance senior à Paris.",
            Email = "marwen.chhaibi.95@gmail.com",
            Phone = "+33 7 53 37 97 07",
            Location = "Paris, France",
            LinkedInUrl = "https://www.linkedin.com/in/medmarwenchhibi/",
            GitHubUrl = "https://github.com/marwenChhaibi",
            AzureDevOpsUrl = "https://dev.azure.com/Urfium/",
            YearsOfExperience = 7,
            OpenToWork = true
        });
        return Task.CompletedTask;
    }

    private static Task SeedExperiencesAsync(AppDbContext db, CancellationToken ct)
    {
        var experiences = new[]
        {
            new Experience
            {
                Id = Guid.NewGuid(),
                Company = "Action Logement",
                Role = "Fullstack .NET Developer — Tech Lead",
                RoleFr = "Développeur Fullstack .NET — Référent Technique",
                CompanyUrl = "https://www.actionlogement.fr",
                Location = "Paris",
                StartDate = new DateOnly(2024, 9, 1),
                IsCurrentPosition = true,
                Description = "Tech lead on a mission-critical social housing platform. Led architecture reviews, mentored 3 junior developers, and delivered V1.1 on schedule in March 2026.",
                DescriptionFr = "Référent technique sur une plateforme de gestion du logement social. Animation des revues d'architecture, mentoring de 3 développeurs juniors et livraison de la V1.1 en mars 2026.",
                TechStack = [".NET 8", "Aurelia.js", "MediatR", "CQRS", "SQL Server", "Azure DevOps"],
                Highlights = ["100% PR reviews", "3 juniors mentored", "V1.1 delivered on schedule (March 2026)", "Architecture standards authored"],
                HighlightsFr = ["100% de PR reviewées", "3 développeurs juniors encadrés", "V1.1 livrée dans les délais (mars 2026)", "Standards d'architecture rédigés"],
                SortOrder = 1
            },
            new Experience
            {
                Id = Guid.NewGuid(),
                Company = "Bouygues Telecom",
                Role = "Backend .NET Developer",
                RoleFr = "Développeur Backend .NET",
                CompanyUrl = "https://www.bouyguestelecom.fr",
                Location = "Paris",
                StartDate = new DateOnly(2023, 6, 1),
                EndDate = new DateOnly(2024, 8, 31),
                IsCurrentPosition = false,
                Description = "Backend developer on microservices handling millions of telecom events. Achieved ≥70% test coverage with zero regressions over 14 months.",
                DescriptionFr = "Développeur backend sur des microservices traitant des millions d'événements télécom. Couverture de tests ≥70% sans régression sur 14 mois.",
                TechStack = [".NET 6/8", "CQRS", "DDD", "Azure Service Bus", "xUnit", "TDD", "Oracle"],
                Highlights = ["≥70% test coverage", "0 regressions in 14 months", "Microservices architecture standards co-authored", "Azure Service Bus event streaming"],
                HighlightsFr = ["≥70% de couverture de tests", "0 régression en 14 mois", "Standards microservices co-rédigés", "Streaming d'événements Azure Service Bus"],
                SortOrder = 2
            },
            new Experience
            {
                Id = Guid.NewGuid(),
                Company = "Equativ",
                Role = "Fullstack .NET / Angular Developer",
                RoleFr = "Développeur Fullstack .NET / Angular",
                CompanyUrl = "https://equativ.com",
                Location = "Paris",
                StartDate = new DateOnly(2022, 6, 1),
                EndDate = new DateOnly(2023, 6, 30),
                IsCurrentPosition = false,
                Description = "Fullstack developer on a high-traffic ad delivery platform serving billions of ad requests. Optimized SQL queries and maintained Angular 8 frontend.",
                DescriptionFr = "Développeur fullstack sur une plateforme de distribution publicitaire à fort trafic. Optimisation SQL et maintenance du frontend Angular 8.",
                TechStack = [".NET 6", "Angular 8", "RxJS", "SQL Server", "GitLab CI"],
                Highlights = ["SQL query optimization", "Angular 8 frontend maintenance", "GitLab CI pipeline improvements", "High-traffic ad platform"],
                HighlightsFr = ["Optimisation des requêtes SQL", "Maintenance du frontend Angular 8", "Amélioration des pipelines GitLab CI", "Plateforme publicitaire à fort trafic"],
                SortOrder = 3
            },
            new Experience
            {
                Id = Guid.NewGuid(),
                Company = "Ecovadis",
                Role = "Backend .NET Developer — Scrum Master",
                RoleFr = "Développeur Backend .NET — Scrum Master",
                CompanyUrl = "https://ecovadis.com",
                Location = "Paris",
                StartDate = new DateOnly(2021, 3, 1),
                EndDate = new DateOnly(2022, 6, 30),
                IsCurrentPosition = false,
                Description = "Backend developer and Scrum Master on a sustainability rating SaaS. Led monolith-to-microservices migration and implemented OpenID Connect SSO via Identity Server.",
                DescriptionFr = "Développeur backend et Scrum Master sur un SaaS de notation RSE. Migration partielle monolithe → microservices et mise en place du SSO OpenID Connect via Identity Server.",
                TechStack = [".NET 5", "GraphQL", "CQRS", "Azure Functions", "Identity Server", "OpenID Connect", "OAuth2"],
                Highlights = ["Scrum Master (all ceremonies)", "Monolith → microservices migration", "Identity Server / SSO implementation", "GraphQL API design"],
                HighlightsFr = ["Scrum Master (toutes les cérémonies)", "Migration monolithe → microservices", "Mise en place Identity Server / SSO", "Conception d'API GraphQL"],
                SortOrder = 4
            },
            new Experience
            {
                Id = Guid.NewGuid(),
                Company = "Octoplus (GROUPAMA)",
                Role = "Fullstack .NET / Vue.js Developer",
                RoleFr = "Développeur Fullstack .NET / Vue.js",
                Location = "Paris",
                StartDate = new DateOnly(2020, 1, 1),
                EndDate = new DateOnly(2021, 3, 31),
                IsCurrentPosition = false,
                Description = "Fullstack developer on an internal insurance management platform. Built gRPC services and Vue.js 2 interfaces for the GROUPAMA subsidiary.",
                DescriptionFr = "Développeur fullstack sur une plateforme de gestion interne d'assurance. Développement de services gRPC et d'interfaces Vue.js 2 pour la filiale GROUPAMA.",
                TechStack = [".NET Core 2.2", "Vue.js 2", "gRPC", "EF Core", "Azure"],
                Highlights = ["gRPC services design", "Vue.js 2 frontend", "Azure deployment", "Insurance domain modelling"],
                HighlightsFr = ["Conception de services gRPC", "Frontend Vue.js 2", "Déploiement Azure", "Modélisation du domaine assurance"],
                SortOrder = 5
            },
            new Experience
            {
                Id = Guid.NewGuid(),
                Company = "Féderys + Axe Finance",
                Role = "Fullstack Developer (Founder's mission + PFE)",
                RoleFr = "Développeur Fullstack (mission fondateur + PFE)",
                Location = "Tunis / Paris",
                StartDate = new DateOnly(2018, 7, 1),
                EndDate = new DateOnly(2019, 12, 31),
                IsCurrentPosition = false,
                Description = "Built internal tools at Féderys then completed engineering thesis (PFE) at Axe Finance with distinction. Developed a real-time financial dashboard using SignalR and Active Directory authentication.",
                DescriptionFr = "Développement d'outils internes chez Féderys puis PFE chez Axe Finance avec mention Très Bien (18/20). Tableau de bord financier temps réel avec SignalR et authentification Active Directory.",
                TechStack = [".NET Core 3.0", "Vue.js", "SignalR", "Active Directory", "SQL Server"],
                Highlights = ["PFE mention Très Bien — 18/20", "Real-time financial dashboard", "SignalR integration", "Active Directory auth"],
                HighlightsFr = ["PFE mention Très Bien — 18/20", "Tableau de bord financier temps réel", "Intégration SignalR", "Authentification Active Directory"],
                SortOrder = 6
            }
        };

        db.Experiences.AddRange(experiences);
        return Task.CompletedTask;
    }

    private static Task SeedSkillsAsync(AppDbContext db, CancellationToken ct)
    {
        int order = 0;
        var skills = new List<Skill>
        {
            new() { Id = Guid.NewGuid(), Category = "Languages", CategoryFr = "Langages", Name = "C#", Level = SkillLevel.Expert, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "Languages", CategoryFr = "Langages", Name = "TypeScript", Level = SkillLevel.Advanced, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "Languages", CategoryFr = "Langages", Name = "JavaScript", Level = SkillLevel.Advanced, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "Languages", CategoryFr = "Langages", Name = "SQL", Level = SkillLevel.Expert, SortOrder = order++ },

            new() { Id = Guid.NewGuid(), Category = "Frameworks", CategoryFr = "Frameworks", Name = "ASP.NET Core", Level = SkillLevel.Expert, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "Frameworks", CategoryFr = "Frameworks", Name = "Vue.js 3", Level = SkillLevel.Advanced, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "Frameworks", CategoryFr = "Frameworks", Name = "MediatR", Level = SkillLevel.Expert, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "Frameworks", CategoryFr = "Frameworks", Name = "EF Core", Level = SkillLevel.Expert, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "Frameworks", CategoryFr = "Frameworks", Name = "Angular", Level = SkillLevel.Intermediate, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "Frameworks", CategoryFr = "Frameworks", Name = "Aurelia.js", Level = SkillLevel.Intermediate, SortOrder = order++ },

            new() { Id = Guid.NewGuid(), Category = "Architecture", CategoryFr = "Architecture", Name = "Clean Architecture", Level = SkillLevel.Expert, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "Architecture", CategoryFr = "Architecture", Name = "CQRS", Level = SkillLevel.Expert, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "Architecture", CategoryFr = "Architecture", Name = "DDD", Level = SkillLevel.Advanced, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "Architecture", CategoryFr = "Architecture", Name = "Microservices", Level = SkillLevel.Advanced, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "Architecture", CategoryFr = "Architecture", Name = "Event-Driven", Level = SkillLevel.Advanced, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "Architecture", CategoryFr = "Architecture", Name = "SOLID", Level = SkillLevel.Expert, SortOrder = order++ },

            new() { Id = Guid.NewGuid(), Category = "API & Security", CategoryFr = "API & Sécurité", Name = "REST", Level = SkillLevel.Expert, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "API & Security", CategoryFr = "API & Sécurité", Name = "gRPC", Level = SkillLevel.Advanced, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "API & Security", CategoryFr = "API & Sécurité", Name = "GraphQL", Level = SkillLevel.Intermediate, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "API & Security", CategoryFr = "API & Sécurité", Name = "OAuth2 / OIDC", Level = SkillLevel.Expert, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "API & Security", CategoryFr = "API & Sécurité", Name = "Identity Server", Level = SkillLevel.Advanced, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "API & Security", CategoryFr = "API & Sécurité", Name = "JWT", Level = SkillLevel.Expert, SortOrder = order++ },

            new() { Id = Guid.NewGuid(), Category = "Databases", CategoryFr = "Bases de données", Name = "PostgreSQL", Level = SkillLevel.Expert, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "Databases", CategoryFr = "Bases de données", Name = "SQL Server", Level = SkillLevel.Expert, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "Databases", CategoryFr = "Bases de données", Name = "Oracle", Level = SkillLevel.Intermediate, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "Databases", CategoryFr = "Bases de données", Name = "Redis", Level = SkillLevel.Intermediate, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "Databases", CategoryFr = "Bases de données", Name = "MongoDB", Level = SkillLevel.Familiar, SortOrder = order++ },

            new() { Id = Guid.NewGuid(), Category = "Messaging & Cloud", CategoryFr = "Messaging & Cloud", Name = "Azure Service Bus", Level = SkillLevel.Advanced, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "Messaging & Cloud", CategoryFr = "Messaging & Cloud", Name = "Azure Functions", Level = SkillLevel.Advanced, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "Messaging & Cloud", CategoryFr = "Messaging & Cloud", Name = "Microsoft Azure", Level = SkillLevel.Advanced, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "Messaging & Cloud", CategoryFr = "Messaging & Cloud", Name = "SignalR", Level = SkillLevel.Expert, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "Messaging & Cloud", CategoryFr = "Messaging & Cloud", Name = "Kafka", Level = SkillLevel.Familiar, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "Messaging & Cloud", CategoryFr = "Messaging & Cloud", Name = "RabbitMQ", Level = SkillLevel.Familiar, SortOrder = order++ },

            new() { Id = Guid.NewGuid(), Category = "DevOps & Quality", CategoryFr = "DevOps & Qualité", Name = "Docker", Level = SkillLevel.Expert, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "DevOps & Quality", CategoryFr = "DevOps & Qualité", Name = "Azure DevOps", Level = SkillLevel.Expert, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "DevOps & Quality", CategoryFr = "DevOps & Qualité", Name = "GitHub Actions", Level = SkillLevel.Advanced, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "DevOps & Quality", CategoryFr = "DevOps & Qualité", Name = "GitLab CI", Level = SkillLevel.Intermediate, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "DevOps & Quality", CategoryFr = "DevOps & Qualité", Name = "xUnit / TDD", Level = SkillLevel.Expert, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "DevOps & Quality", CategoryFr = "DevOps & Qualité", Name = "SonarQube", Level = SkillLevel.Intermediate, SortOrder = order++ },

            new() { Id = Guid.NewGuid(), Category = "Methods", CategoryFr = "Méthodes", Name = "Agile / SCRUM", Level = SkillLevel.Expert, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "Methods", CategoryFr = "Méthodes", Name = "Code Review", Level = SkillLevel.Expert, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "Methods", CategoryFr = "Méthodes", Name = "Architecture Review", Level = SkillLevel.Expert, SortOrder = order++ },
            new() { Id = Guid.NewGuid(), Category = "Methods", CategoryFr = "Méthodes", Name = "SAFe", Level = SkillLevel.Intermediate, SortOrder = order++ },
        };

        db.Skills.AddRange(skills);
        return Task.CompletedTask;
    }

    private static Task SeedProjectsAsync(AppDbContext db, CancellationToken ct)
    {
        var projects = new[]
        {
            new Project
            {
                Id = Guid.NewGuid(),
                Title = "URFF — Order & Eat",
                TitleFr = "URFF — Commander & Déguster",
                Description = "Multi-tenant SaaS restaurant platform with QR menu, real-time kitchen screen, POS, owner backoffice, and super-admin. 2 partner restaurants live in production.",
                DescriptionFr = "Plateforme SaaS multi-tenant pour restaurants avec menu QR, écran cuisine temps réel (SignalR), caisse, backoffice propriétaire et super-admin. 2 restaurants partenaires en production.",
                Type = ProjectType.Personal,
                TechStack = [".NET 8", "Vue.js 3", "SignalR", "Clean Architecture", "CQRS", "PostgreSQL", "Docker", "Nginx", "Azure DevOps"],
                LiveUrl = "https://orderurf.com",
                AzureDevOpsUrl = "https://dev.azure.com/Urfium/",
                SortOrder = 1,
                Features = [
                    new() { Id = Guid.NewGuid(), Label = "Multi-tenant white-label", LabelFr = "Multi-tenant white-label", SortOrder = 1 },
                    new() { Id = Guid.NewGuid(), Label = "Real-time kitchen screen (SignalR)", LabelFr = "Écran cuisine temps réel (SignalR)", SortOrder = 2 },
                    new() { Id = Guid.NewGuid(), Label = "QR menu (no app required)", LabelFr = "Menu QR (sans application)", SortOrder = 3 },
                    new() { Id = Guid.NewGuid(), Label = "POS — Caisse", LabelFr = "Caisse (POS)", SortOrder = 4 },
                    new() { Id = Guid.NewGuid(), Label = "5 Vue.js apps in one monorepo", LabelFr = "5 apps Vue.js dans un monorepo", SortOrder = 5 },
                    new() { Id = Guid.NewGuid(), Label = "Full CI/CD with Azure DevOps", LabelFr = "CI/CD complet Azure DevOps", SortOrder = 6 },
                ]
            },
            new Project
            {
                Id = Guid.NewGuid(),
                Title = "ImmoURF",
                TitleFr = "ImmoURF",
                Description = "SaaS real estate management platform — property/client matching, visit scheduling, SignalR notifications, analytics dashboards. 1 active client in production.",
                DescriptionFr = "Plateforme SaaS de gestion immobilière — matching propriétés/clients, calendrier de visites, notifications SignalR, tableaux de bord analytiques. 1 client actif en production.",
                Type = ProjectType.Personal,
                TechStack = [".NET 8", "Vue.js 3", "SignalR", "PrimeVue", "PostgreSQL", "Docker", "Azure DevOps"],
                LiveUrl = "https://immourf.com",
                AzureDevOpsUrl = "https://dev.azure.com/Urfium/ImmoUrf",
                SortOrder = 2,
                Features = [
                    new() { Id = Guid.NewGuid(), Label = "Property / client matching engine", LabelFr = "Moteur de matching propriétés / clients", SortOrder = 1 },
                    new() { Id = Guid.NewGuid(), Label = "Visit calendar & scheduling", LabelFr = "Calendrier de visites", SortOrder = 2 },
                    new() { Id = Guid.NewGuid(), Label = "Real-time notifications (SignalR)", LabelFr = "Notifications temps réel (SignalR)", SortOrder = 3 },
                    new() { Id = Guid.NewGuid(), Label = "Analytics dashboards", LabelFr = "Tableaux de bord analytiques", SortOrder = 4 },
                    new() { Id = Guid.NewGuid(), Label = "Unit tests + Cobertura coverage reports", LabelFr = "Tests unitaires + rapports Cobertura", SortOrder = 5 },
                    new() { Id = Guid.NewGuid(), Label = "Frontend embedded in API binary (no static server)", LabelFr = "Frontend embarqué dans le binaire API", SortOrder = 6 },
                ]
            },
            new Project
            {
                Id = Guid.NewGuid(),
                Title = "Charchili",
                TitleFr = "Charchili",
                Description = "Classified ads platform for Tunisia (real estate, auto, jobs, 10+ categories) fed by URF Radar, an internal multi-source scraping engine. Growing user base.",
                DescriptionFr = "Plateforme d'annonces classées pour la Tunisie (immobilier, auto, emploi, 10+ catégories) alimentée par URF Radar, moteur de scraping multi-sources. Base d'utilisateurs en croissance.",
                Type = ProjectType.Personal,
                TechStack = [".NET 8", "Vue.js 3", "Tailwind CSS", "PostgreSQL", "AWS S3", "Docker", "Azure DevOps"],
                LiveUrl = "https://charchili.urfidentity.xyz",
                AzureDevOpsUrl = "https://dev.azure.com/Urfium/",
                SortOrder = 3,
                Features = [
                    new() { Id = Guid.NewGuid(), Label = "10+ classified ad categories", LabelFr = "10+ catégories d'annonces", SortOrder = 1 },
                    new() { Id = Guid.NewGuid(), Label = "URF Radar: multi-source scraping engine", LabelFr = "URF Radar : moteur de scraping multi-sources", SortOrder = 2 },
                    new() { Id = Guid.NewGuid(), Label = "AWS S3 file storage", LabelFr = "Stockage de fichiers AWS S3", SortOrder = 3 },
                    new() { Id = Guid.NewGuid(), Label = "Vite env injection at build time", LabelFr = "Injection des variables Vite au build", SortOrder = 4 },
                    new() { Id = Guid.NewGuid(), Label = "OIDC auth via urfIdentity", LabelFr = "Auth OIDC via urfIdentity", SortOrder = 5 },
                ]
            },
            new Project
            {
                Id = Guid.NewGuid(),
                Title = "urfIdentity",
                TitleFr = "urfIdentity",
                Description = "Centralized OAuth2 / OpenID Connect identity server built with OpenIddict and ASP.NET Core. Powers authentication for all URF ecosystem applications.",
                DescriptionFr = "Serveur d'identité OAuth2 / OpenID Connect centralisé, construit avec OpenIddict et ASP.NET Core. Gère l'authentification de l'ensemble de l'écosystème URF.",
                Type = ProjectType.Personal,
                TechStack = [".NET 8", "OpenIddict", "ASP.NET Core Identity", "PostgreSQL", "OAuth2", "OpenID Connect", "Docker"],
                LiveUrl = "https://urfidentity.xyz",
                SortOrder = 4,
                Features = [
                    new() { Id = Guid.NewGuid(), Label = "Authorization Code + PKCE flow", LabelFr = "Flow Authorization Code + PKCE", SortOrder = 1 },
                    new() { Id = Guid.NewGuid(), Label = "Google OAuth social login", LabelFr = "Connexion sociale Google OAuth", SortOrder = 2 },
                    new() { Id = Guid.NewGuid(), Label = "Multi-application client registry", LabelFr = "Registre multi-applications", SortOrder = 3 },
                    new() { Id = Guid.NewGuid(), Label = "Per-app user access control", LabelFr = "Contrôle d'accès par application", SortOrder = 4 },
                    new() { Id = Guid.NewGuid(), Label = "JWKS endpoint for JWT validation", LabelFr = "Endpoint JWKS pour validation JWT", SortOrder = 5 },
                ]
            },
            new Project
            {
                Id = Guid.NewGuid(),
                Title = "UrfFiles",
                TitleFr = "UrfFiles",
                Description = "Centralized file storage service backed by Backblaze B2 and Cloudflare CDN. Provides API key and JWT-protected endpoints for all URF apps.",
                DescriptionFr = "Service de stockage de fichiers centralisé, s'appuyant sur Backblaze B2 et Cloudflare CDN. Endpoints protégés par clé API et JWT pour toutes les apps URF.",
                Type = ProjectType.Personal,
                TechStack = [".NET 8", "Backblaze B2", "Cloudflare CDN", "PostgreSQL", "Docker"],
                SortOrder = 5,
                Features = [
                    new() { Id = Guid.NewGuid(), Label = "Backblaze B2 cloud storage", LabelFr = "Stockage cloud Backblaze B2", SortOrder = 1 },
                    new() { Id = Guid.NewGuid(), Label = "Cloudflare CDN for fast delivery", LabelFr = "CDN Cloudflare pour livraison rapide", SortOrder = 2 },
                    new() { Id = Guid.NewGuid(), Label = "API key + JWT dual authentication", LabelFr = "Double authentification clé API + JWT", SortOrder = 3 },
                    new() { Id = Guid.NewGuid(), Label = "Automatic key rotation", LabelFr = "Rotation automatique des clés", SortOrder = 4 },
                ]
            },
            new Project
            {
                Id = Guid.NewGuid(),
                Title = "UrfRadar",
                TitleFr = "UrfRadar",
                Description = "Internal multi-source scraping and data aggregation engine for the URF ecosystem. Feeds Charchili with classified ads from Tunisian platforms.",
                DescriptionFr = "Moteur interne de scraping multi-sources et d'agrégation de données pour l'écosystème URF. Alimente Charchili en annonces depuis les plateformes tunisiennes.",
                Type = ProjectType.Personal,
                TechStack = [".NET 8", "Clean Architecture", "OIDC auth", "PostgreSQL", "Docker"],
                AzureDevOpsUrl = "https://dev.azure.com/Urfium/",
                SortOrder = 6,
                Features = [
                    new() { Id = Guid.NewGuid(), Label = "Multi-source scraping connectors", LabelFr = "Connecteurs de scraping multi-sources", SortOrder = 1 },
                    new() { Id = Guid.NewGuid(), Label = "Data normalization pipeline", LabelFr = "Pipeline de normalisation des données", SortOrder = 2 },
                    new() { Id = Guid.NewGuid(), Label = "Automated scheduling", LabelFr = "Planification automatisée", SortOrder = 3 },
                ]
            },
            new Project
            {
                Id = Guid.NewGuid(),
                Title = "AutImmo",
                TitleFr = "AutImmo",
                Description = "Real estate automation platform with automatic lead scoring, document generation, and multi-channel notification workflows.",
                DescriptionFr = "Plateforme d'automatisation immobilière avec scoring automatique des leads, génération de documents et workflows de notifications multi-canaux.",
                Type = ProjectType.Personal,
                TechStack = [".NET 8", "Vue.js 3", "PostgreSQL", "Docker"],
                SortOrder = 7,
                Features = [
                    new() { Id = Guid.NewGuid(), Label = "Lead scoring automation", LabelFr = "Automatisation du scoring des leads", SortOrder = 1 },
                    new() { Id = Guid.NewGuid(), Label = "Document generation", LabelFr = "Génération de documents", SortOrder = 2 },
                    new() { Id = Guid.NewGuid(), Label = "Multi-channel notifications", LabelFr = "Notifications multi-canaux", SortOrder = 3 },
                ]
            },

            // ── Professional projects ────────────────────────────────────────
            new Project
            {
                Id = Guid.NewGuid(),
                Title = "Social Housing Platform — Action Logement",
                TitleFr = "Plateforme logement social — Action Logement",
                Description = "Mission-critical platform managing social housing allocation and beneficiary tracking for one of France's largest housing operators. Tech lead on a 4-developer team, delivered V1.1 in March 2026.",
                DescriptionFr = "Plateforme critique de gestion des attributions de logements sociaux pour l'un des plus grands bailleurs de France. Référent technique d'une équipe de 4 développeurs, V1.1 livrée en mars 2026.",
                Type = ProjectType.Professional,
                TechStack = [".NET 8", "Aurelia.js", "MediatR", "CQRS", "Clean Architecture", "SQL Server", "Azure DevOps", "Docker"],
                LiveUrl = "https://www.actionlogement.fr",
                SortOrder = 1,
                IsVisible = true,
                Features = [
                    new() { Id = Guid.NewGuid(), Label = "Tech Lead — architecture reviews & standards authoring", LabelFr = "Référent technique — revues d'architecture et rédaction des standards", SortOrder = 1 },
                    new() { Id = Guid.NewGuid(), Label = "Mentoring 3 junior developers", LabelFr = "Encadrement de 3 développeurs juniors", SortOrder = 2 },
                    new() { Id = Guid.NewGuid(), Label = "100% PR reviews — zero unreviewed merge", LabelFr = "100% des PR reviewées — zéro merge non validé", SortOrder = 3 },
                    new() { Id = Guid.NewGuid(), Label = "V1.1 delivered on schedule (March 2026)", LabelFr = "V1.1 livrée dans les délais (mars 2026)", SortOrder = 4 },
                    new() { Id = Guid.NewGuid(), Label = "CQRS pipeline with MediatR behaviours (validation, logging, caching)", LabelFr = "Pipeline CQRS avec behaviors MediatR (validation, logging, cache)", SortOrder = 5 },
                    new() { Id = Guid.NewGuid(), Label = "Full Azure DevOps CI/CD — build → test → deploy → health check", LabelFr = "CI/CD Azure DevOps complet — build → test → deploy → health check", SortOrder = 6 },
                ]
            },
            new Project
            {
                Id = Guid.NewGuid(),
                Title = "Telecom Microservices — Bouygues Telecom",
                TitleFr = "Microservices télécom — Bouygues Telecom",
                Description = "Backend microservices processing millions of telecom events daily. Achieved ≥70% test coverage on all business-critical paths. Zero production regressions over 14 months of active development.",
                DescriptionFr = "Microservices backend traitant des millions d'événements télécom quotidiennement. Couverture ≥70% sur tous les chemins critiques. Zéro régression en production sur 14 mois.",
                Type = ProjectType.Professional,
                TechStack = [".NET 6", ".NET 8", "CQRS", "DDD", "Azure Service Bus", "xUnit", "TDD", "Oracle", "Docker"],
                LiveUrl = "https://www.bouyguestelecom.fr",
                SortOrder = 2,
                IsVisible = true,
                Features = [
                    new() { Id = Guid.NewGuid(), Label = "Event-driven architecture with Azure Service Bus", LabelFr = "Architecture événementielle via Azure Service Bus", SortOrder = 1 },
                    new() { Id = Guid.NewGuid(), Label = "TDD on all critical paths — ≥70% coverage", LabelFr = "TDD sur tous les chemins critiques — couverture ≥70%", SortOrder = 2 },
                    new() { Id = Guid.NewGuid(), Label = "DDD aggregates — bounded contexts per telecom domain", LabelFr = "Agrégats DDD — bounded contexts par domaine télécom", SortOrder = 3 },
                    new() { Id = Guid.NewGuid(), Label = "Microservices architecture standards co-authored", LabelFr = "Standards d'architecture microservices co-rédigés", SortOrder = 4 },
                    new() { Id = Guid.NewGuid(), Label = "0 production regressions over 14 months", LabelFr = "0 régression en production sur 14 mois", SortOrder = 5 },
                ]
            },
            new Project
            {
                Id = Guid.NewGuid(),
                Title = "Ad Delivery Platform — Equativ",
                TitleFr = "Plateforme publicitaire — Equativ",
                Description = "Fullstack development on a high-traffic ad delivery platform serving billions of ad requests. SQL query optimization and Angular 8 frontend maintenance. CI/CD pipeline improvements on GitLab.",
                DescriptionFr = "Développement fullstack sur une plateforme de distribution publicitaire à fort trafic. Optimisation SQL et maintenance du frontend Angular 8. Amélioration des pipelines GitLab CI.",
                Type = ProjectType.Professional,
                TechStack = [".NET 6", "Angular 8", "RxJS", "SQL Server", "GitLab CI", "Docker"],
                LiveUrl = "https://equativ.com",
                SortOrder = 3,
                IsVisible = true,
                Features = [
                    new() { Id = Guid.NewGuid(), Label = "High-traffic platform — billions of ad requests", LabelFr = "Plateforme à fort trafic — milliards de requêtes publicitaires", SortOrder = 1 },
                    new() { Id = Guid.NewGuid(), Label = "SQL query optimization — significant latency reduction", LabelFr = "Optimisation des requêtes SQL — réduction significative des latences", SortOrder = 2 },
                    new() { Id = Guid.NewGuid(), Label = "Angular 8 / RxJS frontend maintenance and new features", LabelFr = "Maintenance et nouvelles fonctionnalités Angular 8 / RxJS", SortOrder = 3 },
                    new() { Id = Guid.NewGuid(), Label = "GitLab CI pipeline improvements — faster feedback loop", LabelFr = "Amélioration des pipelines GitLab CI — feedback loop accéléré", SortOrder = 4 },
                ]
            },
            new Project
            {
                Id = Guid.NewGuid(),
                Title = "Sustainability Rating SaaS — Ecovadis",
                TitleFr = "SaaS notation RSE — Ecovadis",
                Description = "Backend developer and Scrum Master on a global B2B SaaS platform rating 100k+ companies on CSR criteria. Led partial monolith-to-microservices migration and implemented OpenID Connect SSO.",
                DescriptionFr = "Développeur backend et Scrum Master sur un SaaS B2B mondial évaluant 100k+ entreprises sur des critères RSE. Migration partielle monolithe → microservices et mise en place du SSO OpenID Connect.",
                Type = ProjectType.Professional,
                TechStack = [".NET 5", "GraphQL", "CQRS", "Azure Functions", "Identity Server", "OpenID Connect", "OAuth2", "Docker"],
                LiveUrl = "https://ecovadis.com",
                SortOrder = 4,
                IsVisible = true,
                Features = [
                    new() { Id = Guid.NewGuid(), Label = "Scrum Master — all ceremonies, backlog grooming, retrospectives", LabelFr = "Scrum Master — toutes les cérémonies, grooming, rétrospectives", SortOrder = 1 },
                    new() { Id = Guid.NewGuid(), Label = "Monolith → microservices migration (partial)", LabelFr = "Migration partielle monolithe → microservices", SortOrder = 2 },
                    new() { Id = Guid.NewGuid(), Label = "Identity Server / SSO — OpenID Connect implementation", LabelFr = "Identity Server / SSO — implémentation OpenID Connect", SortOrder = 3 },
                    new() { Id = Guid.NewGuid(), Label = "GraphQL API design and implementation", LabelFr = "Conception et implémentation d'une API GraphQL", SortOrder = 4 },
                    new() { Id = Guid.NewGuid(), Label = "Azure Functions for async background processing", LabelFr = "Azure Functions pour le traitement asynchrone en arrière-plan", SortOrder = 5 },
                ]
            },
            new Project
            {
                Id = Guid.NewGuid(),
                Title = "Insurance Management Platform — GROUPAMA (Octoplus)",
                TitleFr = "Plateforme de gestion assurance — GROUPAMA (Octoplus)",
                Description = "Fullstack developer on an internal insurance management platform for GROUPAMA subsidiary. Built gRPC microservices and Vue.js 2 interfaces for policy management and claims tracking.",
                DescriptionFr = "Développeur fullstack sur une plateforme interne de gestion d'assurance pour la filiale GROUPAMA. Développement de microservices gRPC et d'interfaces Vue.js 2 pour la gestion des contrats et des sinistres.",
                Type = ProjectType.Professional,
                TechStack = [".NET Core 2.2", "Vue.js 2", "gRPC", "EF Core", "Microsoft Azure", "SQL Server"],
                SortOrder = 5,
                IsVisible = true,
                Features = [
                    new() { Id = Guid.NewGuid(), Label = "gRPC microservices — high-performance inter-service communication", LabelFr = "Microservices gRPC — communication inter-services haute performance", SortOrder = 1 },
                    new() { Id = Guid.NewGuid(), Label = "Vue.js 2 frontend — policy and claims management UI", LabelFr = "Frontend Vue.js 2 — interface gestion contrats et sinistres", SortOrder = 2 },
                    new() { Id = Guid.NewGuid(), Label = "Azure deployment — staging and production environments", LabelFr = "Déploiement Azure — environnements staging et production", SortOrder = 3 },
                    new() { Id = Guid.NewGuid(), Label = "Insurance domain modelling with DDD aggregates", LabelFr = "Modélisation du domaine assurance avec agrégats DDD", SortOrder = 4 },
                ]
            },
        };

        db.Projects.AddRange(projects);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken ct) => Task.CompletedTask;
}
