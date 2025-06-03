import { Routes } from '@angular/router';
import { Store } from './features/store/store';
import { History } from './features/history/history';
import { User } from './features/user/user';
export const routes: Routes = [
    {
        path: '',
        component: Store,
    },
    {
        path: 'history',
        component: History,
    }
    ,
    {
        path: 'user',
        component: User,
    }
];
