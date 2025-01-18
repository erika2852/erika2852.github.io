---
title: Laravelでログイン機能を実装する
categories: [Laravel]
tags: [laravel,login]
pin: true
---

Laravel Authを使用してログイン,会員登録機能を実装します。

## 事前準備

- [Laravelプロジェクト](https://erika2852.github.io/posts/LaLavel/)

## 1. Laravel UIパッケージのインストール

```bash
composer require laravel/ui
```

## 2. ログインUIの作成

```bash
php artisan ui vue --auth
```

bootstrap, vue, react の基準の認証 UIを作成できますが、ここではvueを使用します。

```bash
npm install
npm run dev
```
必要なパッケージをインストールして、ログインUIを作成します。

## 3. ログインUIの確認

```bash
php artisan serve
```

サーバを起動したら、http://localhost:8000/login にアクセスしてログイン画面を確認します。

![login](../assets/img/LaravelLogin/login.png?raw=true.png)

## 4. テーブル作成
dockerのmysqlに接続して、ユーザ情報テーブルを作成します。

```sql
CREATE TABLE members (
    customer_id INT AUTO_INCREMENT PRIMARY KEY, -- 顧客ID
    name VARCHAR(255) NOT NULL,                -- 名前
    email VARCHAR(255) UNIQUE NOT NULL,        -- メールアドレス
    password VARCHAR(255) NOT NULL,            -- パスワード
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP, -- 作成日
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP -- 更新日
);
```

## 5. ユーザモデルの作成

ユーザ情報モデルを作成します。

```bash
php artisan make:model Member
```
app/Models/Member.phpが作成されました。

```php
<?php

namespace App\Models;

use Illuminate\Foundation\Auth\User as Authenticatable;
use Illuminate\Notifications\Notifiable;

class Member extends Authenticatable
{
    use Notifiable;

    protected $table = 'members'; 
    protected $primaryKey = 'customer_id'; 

    protected $fillable = [
        'name', 'email', 'password'
    ];

    public $timestamps = true; // created_at, updated_at 

    protected $hidden = [
        'password',
    ];
}
```

内容を修正します。

## 6. config/auth.php修正

config/auth.phpを編集して、ユーザモデルを設定します。

```php
'providers' => [
    'users' => [
        'driver' => 'eloquent',
        'model' => App\Models\Member::class,
    ],
],
```
デフォルトではUsersテーブルを使用しますが、今回は先ほど作成したmembersテーブルを使用するように設定しました。

## 7. RegisterController.php修正

app/Http/Controllers/Auth/RegisterController.phpを編集して、ユーザ情報テーブルを使用するように設定します。

```php
<?php

namespace App\Http\Controllers\Auth;

use App\Http\Controllers\Controller;
use App\Models\Member;
use Illuminate\Foundation\Auth\RegistersUsers;
use Illuminate\Support\Facades\Hash;
use Illuminate\Support\Facades\Validator;
```

```php
/**
     * Create a new user instance after a valid registration.
     *
     * @param  array  $data
     * @return \App\Models\Member
     */
    protected function create(array $data)
    {
        return Member::create([
            'name' => $data['name'],
            'email' => $data['email'],
            'password' => Hash::make($data['password']),
        ]);
    }
```


## 8. 会員登録テスト
http://localhost:8000/register にアクセスして、会員登録画面を確認します。

![register](../assets/img/LaravelLogin/register.png?raw=true.png)

ユーザ登録画面が表示されました。

