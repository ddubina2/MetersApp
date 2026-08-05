import { SUPPORTED_LANGUAGES } from '@i18n';
import { useTranslation } from 'react-i18next';
import { twMerge } from 'tailwind-merge';
import type { FC } from 'react';

export type LanguageSwitcherProps = {
  className?: string;
};

export const LanguageSwitcher: FC<LanguageSwitcherProps> = ({ className }) => {
  const { i18n, t } = useTranslation();
  const currentLanguage = i18n.resolvedLanguage ?? i18n.language;

  return (
    <div
      role='group'
      aria-label={t('header.languageSwitcher')}
      className={twMerge(
        'flex items-center gap-0.5 rounded-lg border border-line bg-raised p-0.5',
        className
      )}
    >
      {SUPPORTED_LANGUAGES.map((language) => {
        const isActive = currentLanguage?.toLowerCase() === language;

        return (
          <button
            key={language}
            type='button'
            aria-pressed={isActive}
            onClick={() => i18n.changeLanguage(language)}
            className={twMerge(
              'rounded-md px-2 py-0.5 text-sm font-semibold uppercase transition-colors',
              'focus:outline-none focus-visible:bg-hover',
              isActive
                ? 'bg-primary text-on-primary'
                : 'text-secondary hover:bg-hover'
            )}
          >
            {language}
          </button>
        );
      })}
    </div>
  );
};
