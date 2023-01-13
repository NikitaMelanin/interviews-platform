export interface IRoute {
  title: string;
  path: string;
  side: boolean;
  roles?: string[];
}

export const mainRoutes: IRoute[] = [
  {title: 'Список вакансий', path: '/dashboard/vacancies', side: true, roles: ['Hr', 'Administrator']},
  {title: ' Список кандидатов', path: '/dashboard/candidates', side: true, roles: ['Hr', 'Administrator']},
  {title: ' Добавить вакансию', path: '/dashboard/vacancy/create', side: false, roles: ['Hr', 'Administrator']},
  {title: ' Добавить HR', path: '/dashboard/hr/create', side: false, roles: ['Administrator']},
];

export const vacancyRoutes = (id: string): IRoute[] => ([
  {title: 'Основная информация', path: '/dashboard/vacancy/' + id + '/main', side: true, roles: ['Hr', 'Administrator']},
  {title: 'Интервью', path: '/dashboard/vacancy/' + id + '/interviews', side: true, roles: ['Hr', 'Administrator']},
]);

export const interviewRoutes = (id: string): IRoute[] => ([
  {title: 'Просмотр', path: '/dashboard/interview/' + id + '/main', side: true, roles: ['Hr', 'Administrator']},
  {title: 'Настройки', path: '/dashboard/interview/' + id + '/settings', side: true, roles: ['Hr', 'Administrator']},
])

// export const candidatesRoutes = (id: string): IRoute[] => ([
//   {title: 'Редактировать', path: '/dashboard/candidates-edit/'+ id + '/main', side: false, roles: ['Hr', 'Administrator']},
// ])
