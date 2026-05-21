const BASE = 'https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons'

const icons: [RegExp, string][] = [
  [/\.net|asp\.net|dotnet|ef core|entity framework|signalr/i, `${BASE}/dotnetcore/dotnetcore-original.svg`],
  [/\bc#\b/i,                                                  `${BASE}/csharp/csharp-original.svg`],
  [/vue/i,                                                     `${BASE}/vuejs/vuejs-original.svg`],
  [/^typescript$|^ts$/i,                                       `${BASE}/typescript/typescript-original.svg`],
  [/^javascript$|^js$/i,                                       `${BASE}/javascript/javascript-original.svg`],
  [/aurelia/i,                                                 `${BASE}/javascript/javascript-original.svg`],
  [/angular/i,                                                 `${BASE}/angular/angular-original.svg`],
  [/rxjs/i,                                                    `${BASE}/rxjs/rxjs-original.svg`],
  [/sql server|mssql/i,                                        `${BASE}/microsoftsqlserver/microsoftsqlserver-original.svg`],
  [/postgresql|postgres/i,                                     `${BASE}/postgresql/postgresql-original.svg`],
  [/oracle/i,                                                  `${BASE}/oracle/oracle-original.svg`],
  [/redis/i,                                                   `${BASE}/redis/redis-original.svg`],
  [/mongodb/i,                                                 `${BASE}/mongodb/mongodb-original.svg`],
  [/docker/i,                                                  `${BASE}/docker/docker-original.svg`],
  [/kubernetes|k8s/i,                                          `${BASE}/kubernetes/kubernetes-original.svg`],
  [/azure devops/i,                                            `${BASE}/azuredevops/azuredevops-original.svg`],
  [/azure|microsoft azure/i,                                   `${BASE}/azure/azure-original.svg`],
  [/^grpc$/i,                                                  `${BASE}/grpc/grpc-original.svg`],
  [/graphql/i,                                                 `${BASE}/graphql/graphql-plain.svg`],
  [/nginx/i,                                                   `${BASE}/nginx/nginx-original.svg`],
  [/github/i,                                                  `${BASE}/github/github-original.svg`],
  [/gitlab/i,                                                  `${BASE}/gitlab/gitlab-original.svg`],
  [/\bgit\b/i,                                                 `${BASE}/git/git-original.svg`],
  [/tailwind/i,                                                `${BASE}/tailwindcss/tailwindcss-original.svg`],
  [/node\.js|nodejs/i,                                         `${BASE}/nodejs/nodejs-original.svg`],
]

export function techIcon(name: string): string | null {
  for (const [re, url] of icons) {
    if (re.test(name)) return url
  }
  return null
}

const companyDomains: Record<string, string> = {
  'Action Logement':         'actionlogement.fr',
  'Bouygues Telecom':        'bouyguestelecom.fr',
  'Equativ':                 'equativ.com',
  'Ecovadis':                'ecovadis.com',
  'Octoplus (GROUPAMA)':     'groupama.fr',
  'Féderys + Axe Finance':   'axefinance.com',
}

export function companyLogo(name: string): string | null {
  const domain = companyDomains[name]
  return domain ? `https://www.google.com/s2/favicons?domain=${domain}&sz=64` : null
}
