import re

pos_avg_temp = []
with open('input.txt') as f:
    with open('ext.txt') as ext:
        ext_line = ext.readline()
        ext_line = re.sub(r'([ ]*([:;?])[ ]*)', ':', ext_line)
        ext_items = re.split(r':', ext_line)
        if len(ext_items) != 2:
            print(f"Кількість полів в додатковому файлі має бути 2")
            print(f'Рядок {ext_line}')
            exit()

        strong_wind_num = ext_items[0]
        num = ext_items[1]

        if re.fullmatch(r'\d+', strong_wind_num) is None:
            print(f'Неправильний формат кількості рядків, в яких сила вітру > 15 ')
            print(f'Значення: {strong_wind_num}')
            exit()

        if re.fullmatch(r'[1-9]\d*', num) is None:
            print(f'Неправильний формат кількості рядків')
            print(f'Значення: {num}')
            exit()

        if strong_wind_num > num:
            print("Кількість рядків з силою вітру > 15 більше загальної")
            print(f'Рядок: {ext_line}')
            exit()

        num = int(num)
        for rec in range(num):
            s = f.readline()
            s = re.sub(r'([ ]*([:;?])[ ]*)', ':', s)
            items = re.split(r':', s)
            if len(items) != 10:
                print(f"Кільксть полів повинна бути 10, надано {len(items)}")
                print(f"Рядок {s}")
                exit()

            for i in items:
                if i == '':
                    print('Поля не повинні бути пустими')
                    print(f'Рядок: {s}')
                    exit()

            year = items[0]
            if re.fullmatch(r'[1-9]\d{3}', year) is None:
                print('Неправильний формат року')
                print(f'Запис: {year}')
                exit()

            max_temp = items[1]
            # має бути рівно 3 символи після крапки
            # можна використати (\d\d\d|\d\d|\d) для точності 1-3 цифри
            if re.fullmatch(r'[+-]?\d+.\d{3}', max_temp) is None:
                print("Неправильний формат температури")
                print(f'Запис: {max_temp}')
                exit()

            day = items[2]
            # не перевіряється відповідность кількості днів місяцю
            if re.fullmatch(r'[1-9]|[1-2][0-9]|30|31', day) is None:
                print("Неправильний формат дня")
                print(f'Запис: {day}')
                exit()

            month = items[3]
            if re.fullmatch(r'[1-9]|10|11|12', month) is None:
                print("Неправильний формат місяця")
                print(f'Запис: {month}')
                exit()

            recip = items[4]
            if re.fullmatch(r'[0-9]\d*', recip) is None:
                print("Неправильний формат кількості опадів")
                print(f'Запис: {recip}')
                exit()

            station_id = items[5]
            if re.fullmatch(r'[0-9a-zA-Z]{5,7}', station_id) is None:
                print('Неправильний формат коду метеостанції')
                print(f'Запис: {station_id}')
                exit()

            min_temp = items[6]
            if re.fullmatch(r'[+-]?\d+.\d{3}', min_temp) is None:
                print("Неправильний формат температури")
                print(f'Запис: {min_temp}')
                exit()

            rel_humidity = items[7]
            if re.fullmatch(r'\d+.\d', rel_humidity) is None:
                print("Неправильний формат відносної вологості")
                print(f'Запис: {rel_humidity}')
                exit()

            wind_force = items[8]
            if re.fullmatch(r'\d+.\d', wind_force) is None:
                print("Неправильний формат сили вітру")
                print(f'Запис: {wind_force}')
                exit()

            avg_temp = items[9]
            if re.fullmatch(r'[+-]?\d+.\d{3}\s?', avg_temp) is None:
                print("Неправильний формат середньої температури")
                print(f'Запис: {avg_temp}')
                exit()

            if avg_temp[-1] == '\n':
                avg_temp = avg_temp[0:-1]
            items[9] = avg_temp
            
            if float(avg_temp) >= 0:
                pos_avg_temp.append(items)

result_file = open("result.txt", 'w')
for record in pos_avg_temp:
    for i in range(0, len(record) - 1):
        result_file.write(record[i] + ';')
    result_file.write(record[-1] + '\n')
