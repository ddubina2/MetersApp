import { useEffect, type FC } from 'react';
import i18n from '@i18n';

export const I18nSync: FC = () => {
  useEffect(() => {
    const syncDocument = () => {
      document.documentElement.lang = i18n.resolvedLanguage ?? i18n.language ?? 'en';
      document.title = i18n.t('header.title');
    };

    syncDocument();
    i18n.on('languageChanged', syncDocument);

    return () => {
      i18n.off('languageChanged', syncDocument);
    };
  }, []);

  return null;
};
